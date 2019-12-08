using Gateway.Services.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gateway.Authentication
{
    public class MicroAuthHandler : AuthenticationHandler<MicroAuthOptions>
    {
        private readonly IUsersClient _usersClient;

        public MicroAuthHandler(IOptionsMonitor<MicroAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, 
            ISystemClock clock, IUsersClient usersClient) : base(options, logger, encoder, clock)
        {
            _usersClient = usersClient;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get Authorization header value
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Cannot read authorization header!"));
            }

            if (!authorization.ToString().StartsWith("MicroAuth "))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // To delete scheme
            string token = authorization.ToString().Substring(10);

            // The auth key from Authorization header check against the configured ones
            if (!_usersClient.GetTokenCorrectness(token).Result)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid token!"));
            }

            var jwtClaims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;

            // Create authenticated user
            var claims = new[] { new Claim("Player", "Player"), jwtClaims.First(c => c.Type == "sub") };
            var identities = new[] { new ClaimsIdentity(claims) };
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
