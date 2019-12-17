using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using User.API.Controllers;
using User.API.Models;
using User.API.Services;
using Xunit;

namespace User.Test
{
    public class UsersControllerTests
    {
        private UsersController usersController;

        public UsersControllerTests()
        {
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(x => x.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AuthResponse());
            usersServiceMock.Setup(x => x.RegisterUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string x, string y) => 
            new ApplicationUser() {
                UserName = x,
                TreasureCount = 0,
                CurrentHP = 3,
                MaxHP = 3
            });
            usersServiceMock.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync((string x) => new ApplicationUser()
            {
                Id = x,
                UserName = "admin",
                TreasureCount = 0,
                MaxHP = 3,
                CurrentHP = 3
            });
            usersServiceMock.Setup(x => x.TreasurePickup(It.IsAny<string>())).ReturnsAsync((string x) => new ApplicationUser()
            {
                Id = x,
                UserName = "admin",
                TreasureCount = 1,
                MaxHP = 3,
                CurrentHP = 3
            });
            usersServiceMock.Setup(x => x.HPLoss(It.IsAny<string>())).ReturnsAsync((string x) => new ApplicationUser()
            {
                Id = x,
                UserName = "admin",
                TreasureCount = 1,
                MaxHP = 3,
                CurrentHP = 2
            });
            usersServiceMock.Setup(x => x.RoomChange(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((string x, int y) => 
            new ApplicationUser() {
                Id = x,
                UserName = "admin",
                TreasureCount = 1,
                MaxHP = 3,
                CurrentHP = 2,
                CurrentRoomId = y
            });
            usersServiceMock.Setup(x => x.GetLeaderboard(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<LeaderboardUser>() 
            { new LeaderboardUser()
            {
                Username = "admin",
                TreasureCount = 1,
                IsDead = false
            }
            });

            usersController = new UsersController(usersServiceMock.Object);
        }

        [Fact]
        public async void CheckTokenTest()
        {
            var result = await usersController.CheckToken();
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
        }

        [Fact]
        public async void AuthTest()
        {
            var result = await usersController.Authenticate(new Login() { Username = "admin", Password = "admin" });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
        }

        [Fact]
        public async void RegisterTest()
        {
            var result = await usersController.Register(new Login() { Username = "admin", Password = "admin" });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal("admin", (okRes.Value as ApplicationUser).UserName);
        }

        [Fact]
        public async void GetUserByIdTest()
        {
            var result = await usersController.GetUserById("1");
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal("1", (okRes.Value as ApplicationUser).Id);
        }

        [Fact]
        public async void GetLeaderboardTest()
        {
            var result = await usersController.GetLeaderboard();
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
        }

        [Fact]
        public async void TreasurePickupTest()
        {
            var result = await usersController.TreasurePickup("1");
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as ApplicationUser).TreasureCount);
        }

        [Fact]
        public async void HpLossTest()
        {
            var result = await usersController.HPLoss("1");
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(2, (okRes.Value as ApplicationUser).CurrentHP);
        }

        [Fact]
        public async void RoomChangeTest()
        {
            var result = await usersController.RoomChange("1", new RoomIdWrapper() { RoomId = 1 });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as ApplicationUser).CurrentRoomId);
        }
    }
}
