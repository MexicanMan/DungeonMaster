using Gateway.Helpers;
using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public interface IRoomsClient
    {
        Task<Room> GetRoomById(int roomId);
        Task<RoomWrapper> PostCreateRoom(string discovererId, Direction fromDir, int? fromRoomId);
        Task<Room> PatchPutMonster(int roomId, int monsterId);
        Task<Room> PatchPickupTreasure(int roomId);
        Task<Room> DeleteMonster(int roomId);
        Task<Room> GetRandomRoom(Direction direction);
        Task<Room> PatchOpenDoor(int roomId, Direction toDir, int toRoomId);
    }
}
