using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public interface IUsersClient
    {
        Task<bool> GetTokenCorrectness(string token);
        Task<Player> PostRegister(string username, string pwd);
        Task<AuthResponse> PostAuth(string username, string pwd);
        Task<Player> GetPlayerById(string id);
        Task<Player> PatchRoomChange(string id, int roomId);
        Task<Player> PatchPickupTreasure(string id);
        Task<Player> PatchHpLoss(string id);
        Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page);
        Task GetFailing();
    }
}
