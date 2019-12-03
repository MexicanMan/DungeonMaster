using Gateway.Helpers;
using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface ILab4Service
    {
        Task<Player> GetPlayer(string userId);
        Task<GameData> GetDegrRoom(string userId);
        Task<GameData> AttackMonster(string userId);
    }
}
