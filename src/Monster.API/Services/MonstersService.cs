using Microsoft.Extensions.Options;
using Monster.API.Configs;
using Monster.API.Exceptions;
using Monster.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Services
{
    public class MonstersService : IMonstersService
    {
        private readonly MonsterContext _monsterContext;
        private readonly MonsterTypeChancesSettings _chanceSettings;
        private readonly Random _random;

        public MonstersService(MonsterContext monsterContext, IOptions<AppSettings> appSettingsAccessor)
        {
            _monsterContext = monsterContext;
            _chanceSettings = appSettingsAccessor.Value.MonsterTypeChances;

            _random = new Random();
        }

        public async Task<int> CreateRandomMonster()
        {
            int type = 1;
            double prob = _random.NextDouble() * 100;
            if (_chanceSettings.Low <= prob && prob < _chanceSettings.Low + _chanceSettings.Medium)
                type = 2;
            else if (_chanceSettings.Low + _chanceSettings.Medium <= prob && prob < 100)
                type = 3;

            MonsterModel monster = new MonsterModel
            {
                Type = type,
                MaxHP = type,
                CurrentHP = type
            };

            try
            {
                _monsterContext.Monsters.Add(monster);
                await _monsterContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e);
            }

            return monster.MonsterId;
        }

        public async Task<MonsterModel> GetMonster(int id)
        {
            MonsterModel monster = _monsterContext.Monsters.SingleOrDefault(m => m.MonsterId == id);

            return monster;
        }

        public async Task<bool> AttackMonster(int id)
        {
            bool isDead = false;
            MonsterModel monster = _monsterContext.Monsters.SingleOrDefault(m => m.MonsterId == id);

            if (monster != null)
            {
                monster.CurrentHP -= 1;

                if (monster.CurrentHP <= 0)
                    isDead = true;

                try
                {
                    _monsterContext.Monsters.Update(monster);
                    await _monsterContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.Message, e);
                }
            }
            else
            {
                throw new NoSuchMonsterException("No such monster!");
            }

            return isDead;
        }

        public async Task RemoveMonster(int id)
        {
            MonsterModel monster = _monsterContext.Monsters.SingleOrDefault(m => m.MonsterId == id);

            if (monster != null)
            {
                try
                {
                    _monsterContext.Monsters.Remove(monster);
                    await _monsterContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.Message, e);
                }
            }
            else
            {
                throw new NoSuchMonsterException("No such monster!");
            }
        }
    }
}
