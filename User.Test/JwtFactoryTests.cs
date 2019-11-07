using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using User.API.Models;
using User.API.Services;
using Xunit;

namespace User.Test
{
    public class JwtFactoryTests
    {
        private JwtFactory jwtFactory;

        public JwtFactoryTests()
        {
            string secretKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var jwtIssuerOptions = Options.Create(new JwtIssuerOptions()
            {
                Issuer = "api",
                Audience = "http://localhost:5000",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            });

            jwtFactory = new JwtFactory(jwtIssuerOptions);
        }

        [Fact]
        public void GenerateClaimsTest()
        {
            var claims = jwtFactory.GenerateClaimsIdentity("admin", "1");

            Assert.NotNull(claims);
            Assert.Equal("1", claims.FindFirst("Id").Value);
        }

        [Fact]
        public async void GenerateEncodedTokenTest()
        {
            var claims = jwtFactory.GenerateClaimsIdentity("admin", "1");
            var result = await jwtFactory.GenerateEncodedToken("admin", claims);

            Assert.NotNull(result);
        }
    }
}
