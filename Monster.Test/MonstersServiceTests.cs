using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Monster.API.Configs;
using Monster.API.Models;
using Monster.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Monster.Test
{
    public class MonstersServiceTests
    {
        private MonstersService monstersService;

        public MonstersServiceTests()
        {
            List<MonsterModel> monsters = new List<MonsterModel>
            {
                  new MonsterModel()
                  {
                    MonsterId = 1,
                    Type = 2,
                    CurrentHP = 2,
                    MaxHP = 2,
                  }
            };

            var monsterContextMock = new Mock<MonsterContext>(new DbContextOptions<MonsterContext>());
            monsterContextMock.Object.Monsters = GetQueryableMockDbSet(monsters);
            monsterContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            var appSettingsOptions = Options.Create(new AppSettings() { MonsterTypeChances = new MonsterTypeChancesSettings() 
            {
                Low = 60,
                Medium = 30,
                High = 10
            } });

            monstersService = new MonstersService(monsterContextMock.Object, appSettingsOptions);
        }

        [Fact]
        public async void GetMonsterTest()
        {
            var result = await monstersService.GetMonster(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.MonsterId);
        }

        [Fact]
        public async void AttackMonsterTest()
        {
            var result = await monstersService.AttackMonster(1);
            Assert.False(result);

            result = await monstersService.AttackMonster(1);
            Assert.True(result);
        }



        private DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
    }
}
