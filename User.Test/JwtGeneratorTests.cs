using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using User.API.Models;
using User.API.Services;
using Xunit;

namespace User.Test
{
    public class JwtGeneratorTests
    {
        private IJwtFactory jwtFactory;
        private JwtIssuerOptions jwtOptions;

        public JwtGeneratorTests()
        {
            string secretKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var jwtIssuerOptions = Options.Create(new JwtIssuerOptions() 
            { 
                Issuer = "api",
                Audience = "http://localhost:5000",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            });

            jwtOptions = jwtIssuerOptions.Value;
            var jwtFactoryMock = new Mock<IJwtFactory>();
            jwtFactoryMock.Setup(x => x.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<ClaimsIdentity>())).ReturnsAsync("token");
            jwtFactory = jwtFactoryMock.Object;
        }

        [Fact]
        public async void GenerateJwtTest()
        {
            var claims = new ClaimsIdentity(new[] { new Claim("id", "1") });
            var jwt = await JwtGenerator.GenerateJwt(claims, jwtFactory, "admin", jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            Assert.NotNull(jwt);
        }
    }
}
