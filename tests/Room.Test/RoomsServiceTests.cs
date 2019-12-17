using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Room.API;
using Room.API.Configs;
using Room.API.Models;
using Room.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Room.Test
{
    public class RoomsServiceTests
    {
        private RoomsService roomsService;

        public RoomsServiceTests()
        {
            List<RoomModel> rooms = new List<RoomModel>
            {
                  new RoomModel()
                  {
                    RoomId = 1,
                    MonsterId = 1,
                    Treasure = true,
                    EastRoom = true,
                    WestRoom = true,
                    NorthRoom = true,
                    SouthRoom = false,
                    EastRoomId = 1,
                    WestRoomId = 1,
                    NorthRoomId = null,
                    SouthRoomId = null,
                    DiscovererId = "1"
                  }
            };

            var monsterContextMock = new Mock<RoomContext>(new DbContextOptions<RoomContext>());
            monsterContextMock.Object.Rooms = GetQueryableMockDbSet(rooms);
            monsterContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            var appSettingsOptions = Options.Create(new AppSettings()
            {
                MonsterChance = 70,
                TreasureChance = 75,
                ThirdDoorChance = 30,
                FourthDoorChance = 10
            });

            roomsService = new RoomsService(monsterContextMock.Object, appSettingsOptions);
        }

        [Fact]
        public async void CreateRoomTest()
        {
            var result = await roomsService.CreateRoom("1", Direction.None, null);

            Assert.NotNull(result);
            Assert.Equal("1", result.Room.DiscovererId);
        }

        [Fact]
        public async void MosterPutTest()
        {
            var result = await roomsService.MosterPut(1, 2);

            Assert.NotNull(result);
            Assert.Equal(2, result.MonsterId);
        }

        [Fact]
        public async void GetRandomTest()
        {
            var result = await roomsService.GetRandom(Direction.South);

            Assert.NotNull(result);
            Assert.True(result.NorthRoom);
        }

        [Fact]
        public async void GetTest()
        {
            var result = await roomsService.Get(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RoomId);
        }

        [Fact]
        public async void DoorOpenTest()
        {
            var result = await roomsService.DoorOpen(1, Direction.North, 1);

            Assert.NotNull(result);
            Assert.True(result.NorthRoom);
            Assert.Equal(1, result.NorthRoomId);
        }

        [Fact]
        public async void TreasurePickupTest()
        {
            var result = await roomsService.TreasurePickup(1);

            Assert.NotNull(result);
            Assert.False(result.Treasure);
        }

        [Fact]
        public async void MonsterDeletionTest()
        {
            var result = await roomsService.MonsterDeletion(1);

            Assert.NotNull(result);
            Assert.Null(result.MonsterId);
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
            dbSet.Setup(d => d.Update(It.IsAny<T>())).Callback<T>((s) => sourceList[sourceList.IndexOf(s)] = s);

            return dbSet.Object;
        }
    }
}
