using Gateway.Configs;
using Gateway.Exceptions;
using Gateway.Helpers;
using Gateway.Models;
using Gateway.Services.Clients;
using Gateway.Storage;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class Lab4Service : ILab4Service
    {
        private readonly IUsersClient _usersClient;
        private readonly IRoomsClient _roomsClient;
        private readonly IMonstersClient _monstersClient;
        private readonly PendingRequestsStorage _requestsStorage;

        private readonly GameLogicSettings _gameLogicSettings;
        private Random _random;

        public Lab4Service(IUsersClient usersClient, IRoomsClient roomsClient, IMonstersClient monstersClient,
            IOptions<AppSettings> appSettingsAccessor, PendingRequestsStorage requestsStorage)
        {
            _usersClient = usersClient;
            _roomsClient = roomsClient;
            _monstersClient = monstersClient;
            _requestsStorage = requestsStorage;

            _gameLogicSettings = appSettingsAccessor.Value.GameLogic;
            _random = new Random();
        }

        public async Task<Player> GetPlayer(string userId)
        {
            Player player = await _usersClient.GetPlayerById(userId);

            return player;
        }

        public async Task<GameData> GetDegrRoom(string userId)
        {
            Player player = null;
            try
            {
                player = await GetPlayer(userId);
            }
            catch(Exception )
            {
                return new GameData()
                {
                    Room = null,
                    Player = null,
                    Monster = null
                };
            }

            Room room = null;
            try
            {
                room = await _roomsClient.GetRoomById((int)player.CurrentRoomId);
            }
            catch (Exception)
            {
                return new GameData()
                {
                    Room = null,
                    Player = player,
                    Monster = null
                };
            }

            GameData data = new GameData()
            {
                Room = room,
                Player = player,
                Monster = null
            };

            if (room.MonsterId != null)
            {
                try
                {
                    data.Monster = await _monstersClient.GetMonsterById((int)room.MonsterId);
                }
                catch(Exception)
                { }
            }

            return data;
        }

        public async Task<GameData> AttackMonster(string userId)
        {
            AttackMonsterInfo attackMonsterInfo = new AttackMonsterInfo() { UserId = userId };

            try
            {
                Player player = await GetPlayer(userId);

                if (player.CurrentRoomId != null)
                {
                    Room room = await _roomsClient.GetRoomById((int)player.CurrentRoomId);
                    if (room.MonsterId != null)
                    {
                        // Attack monster by player
                        bool isDead = await _monstersClient.PatchAttackMonster((int) room.MonsterId);
                        attackMonsterInfo.isMonsterHPUpdated = true;
                        Monster monster = null;

                        // If it didn't died it attack player too
                        player = await _usersClient.PatchHpLoss(userId);
                        attackMonsterInfo.isUserHPUpdated = true;
                        monster = await _monstersClient.GetMonsterById((int) room.MonsterId);

                        return new GameData() { Player = player, Room = room, Monster = monster };
                    }
                    else
                        throw new NullReferenceException("No alive monsters in the room!");
                }
                else
                    throw new NullReferenceException("Player has no room!");
            }
            catch(BrokenCircuitException)
            {
                _requestsStorage.AttackMonsterRequestQueue.Enqueue(attackMonsterInfo);

                return null;
            }
        }
    }
}
