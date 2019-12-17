using Room.API.Models;
using Room.API.Models.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Services
{
    public interface IRoomsService
    {
        Task<RoomAnswer> CreateRoom(string discovererId, Direction discovererDirection, int? discovererFromRoomId);
        Task<RoomModel> MosterPut(int roomId, int monsterId);
        Task<RoomModel> GetRandom(Direction fromDirection);
        Task<RoomModel> Get(int id);
        Task<RoomModel> DoorOpen(int thisRoomId, Direction dir, int toRoomId);
        Task<RoomModel> TreasurePickup(int id);
        Task<RoomModel> MonsterDeletion(int id);
    }
}
