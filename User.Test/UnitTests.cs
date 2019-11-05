using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using User.API.Configs;
using User.API.Controllers;
using User.API.Models;
using User.API.Services;
using Xunit;

namespace User.Test
{
    public class UnitTests
    {
        private UsersService service;

        public UnitTests()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
            store.Setup(x => x.FindByIdAsync("1", CancellationToken.None))
                .ReturnsAsync(new ApplicationUser()
                {
                    UserName = "admin",
                    Id = "1"
                });

            var mgr = new UserManager<ApplicationUser>(store.Object, null, null, null, null, null, null, null, null);
            var appSettingsOptions = Options.Create(new AppSettings() { MaxPlayerHP = 3 });
            var jwtIssuerOptions = Options.Create(new JwtIssuerOptions() { Issuer = "api", Audience = "http://localhost:5000" });

            service = new UsersService(mgr, appSettingsOptions, jwtIssuerOptions, null);
        }

        [Fact]
        public async void GetUserByIdTest()
        {
            var result = await service.GetUserById("1");

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }
    }
}
