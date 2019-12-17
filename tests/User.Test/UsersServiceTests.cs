using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using User.API.Configs;
using User.API.Controllers;
using User.API.Models;
using User.API.Services;
using Xunit;

namespace User.Test
{
    public class UsersServiceTests
    {
        private UsersService userService;

        public UsersServiceTests()
        {
            List<ApplicationUser> users = new List<ApplicationUser>
            {
                  new ApplicationUser() 
                  { 
                    UserName = "admin",
                    Id = "1",
                    TreasureCount = 0,
                    CurrentHP = 3,
                    MaxHP = 3,
                    CurrentRoomId = 1 
                  },
            };

            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();

            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(users.Find(u => u.Id == "1"));
            mgr.Setup(x => x.Users).Returns(users.AsQueryable());
            mgr.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => users.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var appSettingsOptions = Options.Create(new AppSettings() { MaxPlayerHP = 3 });
            var jwtIssuerOptions = Options.Create(new JwtIssuerOptions() { Issuer = "api", Audience = "http://localhost:5000" });

            userService = new UsersService(mgr.Object, appSettingsOptions, jwtIssuerOptions, null);
        }

        [Fact]
        public async void GetUserByIdTest()
        {
            var result = await userService.GetUserById("1");

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public async void TreasurePickupTest()
        {
            var prev = await userService.GetUserById("1");
            int prevTreasureCount = prev.TreasureCount;
            var result = await userService.TreasurePickup("1");
            
            Assert.NotNull(result);
            Assert.Equal(prevTreasureCount + 1, result.TreasureCount);
        }

        [Fact]
        public async void RegisterTest()
        {
            var result = await userService.RegisterUser("test", "test");

            Assert.NotNull(result);
            Assert.Equal("test", result.UserName);
        }

        [Fact]
        public async void HPLossTest()
        {
            var prev = await userService.GetUserById("1");
            int prevHP = prev.CurrentHP;
            var result = await userService.HPLoss("1");

            Assert.NotNull(result);
            Assert.Equal(prevHP - 1, result.CurrentHP);
        }

        [Fact]
        public async void RoomChangeTest()
        {
            var result = await userService.RoomChange("1", 2);

            Assert.NotNull(result);
            Assert.Equal(2, result.CurrentRoomId);
        }

        [Fact]
        public async void LeaderboardTest()
        {
            var result = await userService.GetLeaderboard(0, 10);
            var listResult = result.ToList();

            Assert.NotNull(result);
            Assert.Equal("admin", listResult[0].Username);
        }
    }
}
