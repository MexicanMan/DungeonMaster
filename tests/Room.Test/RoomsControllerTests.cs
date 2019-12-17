using Microsoft.AspNetCore.Mvc;
using Moq;
using Room.API;
using Room.API.Controllers;
using Room.API.Models;
using Room.API.Models.TransferModels;
using Room.API.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Room.Test
{
    public class RoomsControllerTests
    {
        private RoomsController roomsController;

        public RoomsControllerTests()
        {
            var roomsServiceMock = new Mock<IRoomsService>();
            roomsServiceMock.Setup(x => x.CreateRoom(It.IsAny<string>(), It.IsAny<Direction>(), It.IsAny<int?>())).ReturnsAsync(
                (string x, Direction y, int? z) => new RoomAnswer()
                {
                    Room = new RoomModel() { DiscovererId = x },
                    IsMonsterNeeded = false
                });
            roomsServiceMock.Setup(x => x.MosterPut(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((int x, int y) =>
            new RoomModel()
            {
                RoomId = x,
                MonsterId = y
            });
            roomsServiceMock.Setup(x => x.GetRandom(It.IsAny<Direction>())).ReturnsAsync(new RoomModel());
            roomsServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((int x) => new RoomModel() { RoomId = x });
            roomsServiceMock.Setup(x => x.DoorOpen(It.IsAny<int>(), It.IsAny<Direction>(), It.IsAny<int>())).ReturnsAsync(
                (int x, Direction y, int z) => new RoomModel() { RoomId = x });
            roomsServiceMock.Setup(x => x.TreasurePickup(It.IsAny<int>())).ReturnsAsync(
                (int x) => new RoomModel() { RoomId = x, Treasure = false });
            roomsServiceMock.Setup(x => x.MonsterDeletion(It.IsAny<int>())).ReturnsAsync(
                (int x) => new RoomModel() { RoomId = x, MonsterId = null });

            roomsController = new RoomsController(roomsServiceMock.Object);
        }

        [Fact]
        public async void CreateRoomTest()
        {
            var result = await roomsController.CreateRoom(new Discoverer() { 
                DiscovererId = "1",
                FromDirection = Direction.None,
                FromRoomId = null
            });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal("1", (okRes.Value as RoomAnswer).Room.DiscovererId);
        }

        [Fact]
        public async void MonsterPutTest()
        {
            var result = await roomsController.MonsterPut(1, new Monster() { MonsterId = 1 });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as RoomModel).MonsterId);
        }

        [Fact]
        public async void GetRandomTest()
        {
            var result = await roomsController.GetRandom("west");
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.NotNull((okRes.Value as RoomModel));
        }

        [Fact]
        public async void GetTest()
        {
            var result = await roomsController.Get(1);
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as RoomModel).RoomId);
        }

        [Fact]
        public async void OpenDoorTest()
        {
            var result = await roomsController.OpenDoor(1, new DoorOpen() { Direction = Direction.West, ToRoomId = 1 });
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Equal(1, (okRes.Value as RoomModel).RoomId);
        }

        [Fact]
        public async void TreasurePickupTest()
        {
            var result = await roomsController.TreasurePickup(1);
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.False((okRes.Value as RoomModel).Treasure);
        }

        [Fact]
        public async void MonsterDeletionTest()
        {
            var result = await roomsController.MonsterDeletion(1);
            var okRes = result as OkObjectResult;

            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);
            Assert.Null((okRes.Value as RoomModel).MonsterId);
        }
    }
}
