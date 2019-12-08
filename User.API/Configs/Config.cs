using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace User.API.Configs
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SinglePage",
                    ClientSecrets = { new Secret("secret".ToSha256()) },
                    Enabled = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    //AuthorizationCodeLifetime = 300,
                    AccessTokenLifetime = 60,
                   
                    RedirectUris = {
                        "https://localhost:8081/oacallback",
                        //"http://localhost:5100/silentRenew.html",
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:8080/"
                    },
                    AllowedCorsOrigins = { "http://localhost:8080", "https://localhost:8081" },
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                }
            };
        }
    }
}