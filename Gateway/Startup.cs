using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Authentication;
using Gateway.Configs;
using Gateway.Services;
using Gateway.Services.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            });
            services.AddHttpClient<IRoomsClient, RoomsClient>(client =>
            {
                client.BaseAddress = new Uri(appSettingsSection.Get<AppSettings>().RoomsAPIUrl);
                client.DefaultRequestHeaders.Add("User-Agent", appSettingsSection.Get<AppSettings>().UserAgent);
            });
            services.AddHttpClient<IMonstersClient, MonstersClient>(client =>
            {
                client.BaseAddress = new Uri(appSettingsSection.Get<AppSettings>().MonstersAPIUrl);
                client.DefaultRequestHeaders.Add("User-Agent", appSettingsSection.Get<AppSettings>().UserAgent);
            });

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
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Player", policy => policy.RequireClaim("Player"));
            });

            services.AddCors();

            services.AddControllers();

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IAuthService, AuthService>();
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
    }
}
