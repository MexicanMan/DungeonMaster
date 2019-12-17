using Gateway.Helpers;
using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IGameService
    {
        Task<Player> GetPlayer(string userId);
        Task<GameData> EnterGame(string userId);
        Task<Room> CreateNewRoom(string discovererId, Direction fromDir, int? fromRoomId);
        Task<GameData> PickupTreasure(string userId);
        Task<GameData> AttackMonster(string userId);
        Task<GameData> Move(string userId, Direction to);
        Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page);
    }
}
