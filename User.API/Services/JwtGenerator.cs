using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Services
{
    public static class JwtGenerator
    {
        public static async Task<AuthResponse> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new AuthResponse()
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                Auth_token = await jwtFactory.GenerateEncodedToken(identity.Claims.Single(c => c.Type == "id").Value, identity),
                Expires_in = (int) jwtOptions.ValidFor.TotalSeconds
            };

            return response;
        }
    }
}
