using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Monster.API.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Monster.API.Authentication
{
    public class MonsterAuthHandler : AuthenticationHandler<MonsterAuthOptions>
    {
        private readonly TokenStorage _tokenStorage;

        public MonsterAuthHandler(IOptionsMonitor<MonsterAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, 
            ISystemClock clock, TokenStorage tokenStorage) : base(options, logger, encoder, clock)
        {
            _tokenStorage = tokenStorage;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get Authorization header value
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Cannot read authorization header!"));
            }

            if (!authorization.ToString().StartsWith("MonsterAuth "))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // To delete scheme
            string token = authorization.ToString().Substring(12);

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Int32.Parse(token.Split('.')[0])).ToUniversalTime();

            // The auth key from Authorization header check against the configured ones
            if (!_tokenStorage.activeTokens.Exists(t => t == token) || DateTime.UtcNow > dtDateTime)
            {
                _tokenStorage.activeTokens.Remove(token);
                return Task.FromResult(AuthenticateResult.Fail("Invalid token!"));
            }
            
            // Create authenticated user
            Claim[] claims = new[] { new Claim("Gateway", "Gateway") };
            var identities = new[] { new ClaimsIdentity(claims) };
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
