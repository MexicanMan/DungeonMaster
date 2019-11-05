using Gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services.Clients
{
    public interface IMonstersClient
    {
        Task<int> PostCreateMonster();
        Task<Monster> GetMonsterById(int id);
        Task<bool> PatchAttackMonster(int monsterId);
        Task<bool> DeleteMonster(int monsterId);
    }
}
