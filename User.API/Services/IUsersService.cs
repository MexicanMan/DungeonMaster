using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Services
{
    public interface IUsersService
    {
        Task<string> Auth(string username, string pwd);
        Task<ApplicationUser> RegisterUser(string username, string pwd);
        Task<ApplicationUser> GetUserById(string id);
        Task<ApplicationUser> TreasurePickup(string id);
        Task<ApplicationUser> HPLoss(string id);
        Task<ApplicationUser> RoomChange(string id, int roomId);
        Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page, int pageSize);
    }
}
