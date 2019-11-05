using Monster.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Services
{
    public interface IMonstersService
    {
        Task<int> CreateRandomMonster();
        Task<MonsterModel> GetMonster(int id);
        Task<bool> AttackMonster(int id);
        Task RemoveMonster(int id);
    }
}
