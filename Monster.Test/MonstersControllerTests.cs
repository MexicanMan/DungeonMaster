using Microsoft.AspNetCore.Mvc;
using Monster.API.Controllers;
using Monster.API.Models;
using Monster.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monster.Test
{
    public class MonstersControllerTests
    {
        private MonstersController monstersController;

        public MonstersControllerTests()
        {
            var monstersServiceMock = new Mock<IMonstersService>();
            monstersServiceMock.Setup(x => x.CreateRandomMonster()).ReturnsAsync(1);
            monstersServiceMock.Setup(x => x.GetMonster(It.IsAny<int>())).ReturnsAsync((int x) =>
            new MonsterModel()
            {
                MonsterId = x,
                Type = 1,
                CurrentHP = 1,
                MaxHP = 1
            });
            monstersServiceMock.Setup(x => x.AttackMonster(It.IsAny<int>())).ReturnsAsync(false);
            monstersServiceMock.Setup(x => x.RemoveMonster(It.IsAny<int>()));

            monstersController = new MonstersController(monstersServiceMock.Object);
        }

        [Fact]
        public async void CreateRandomMonsterTest()
        {
            var result = await monstersController.CreateRandomMonster();
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, okRes.Value);
        }

        [Fact]
        public async void GetMonsterTest()
        {
            var result = await monstersController.GetMonster(1);
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as MonsterModel).MonsterId);
        }

        [Fact]
        public async void AttackMonsterTest()
        {
            var result = await monstersController.AttackMonster(1);
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
        }

        [Fact]
        public async void RemoveMonsterTest()
        {
            var result = await monstersController.RemoveMonster(1);
            var okRes = result as OkResult;

            Assert.Equal(200, okRes.StatusCode);
        }
    }
}
