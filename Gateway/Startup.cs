using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gateway.Authentication;
using Gateway.Configs;
using Gateway.Services;
using Gateway.Services.Clients;
using Gateway.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Microsoft.IdentityModel.Logging;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddHttpClient<IUsersClient, UsersClient>(client =>
            {
                client.BaseAddress = new Uri(appSettingsSection.Get<AppSettings>().UsersAPIUrl);
                client.DefaultRequestHeaders.Add("User-Agent", appSettingsSection.Get<AppSettings>().UserAgent);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            services.AddHttpClient<IRoomsClient, RoomsClient>(client =>
            {
                client.BaseAddress = new Uri(appSettingsSection.Get<AppSettings>().RoomsAPIUrl);
                client.DefaultRequestHeaders.Add("User-Agent", appSettingsSection.Get<AppSettings>().UserAgent);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            services.AddHttpClient<IMonstersClient, MonstersClient>(client =>
            {
                client.BaseAddress = new Uri(appSettingsSection.Get<AppSettings>().MonstersAPIUrl);
                client.DefaultRequestHeaders.Add("User-Agent", appSettingsSection.Get<AppSettings>().UserAgent);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = MicroAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = MicroAuthOptions.DefaultScheme; 
            })
                .AddMicroAuth(options => { })
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = "http://localhost:5010/";
                    options.ApiName = "api1";
                    options.RequireHttpsMetadata = false;
                    options.JwtValidationClockSkew = TimeSpan.Zero;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Player", policy => policy.RequireClaim("Player"));
            });

            services.AddSingleton(new PendingRequestsStorage());
            services.AddHostedService<PendingRequestsService>();

            services.AddCors();

            services.AddControllers();

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<ServicesAuth>();

            services.AddTransient<ILab4Service, Lab4Service>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30), (ex, dur) => { Console.WriteLine("CB opened"); }, () => { Console.WriteLine("CB reset"); });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine("Retry attempt!");
                });
        }
    }
}
