using Gateway.Models;
using Gateway.Services.Clients;
using Gateway.Storage;
using Microsoft.Extensions.Hosting;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class PendingRequestsService : BackgroundService
    {
        private const int TIME_DELAY = 1000;

        private readonly PendingRequestsStorage _requestsStorage;
        private readonly IUsersClient _usersClient;
        private readonly IRoomsClient _roomsClient;
        private readonly IMonstersClient _monstersClient;

        public PendingRequestsService(PendingRequestsStorage requestsStorage, IUsersClient usersClient, 
            IRoomsClient roomsClient, IMonstersClient monstersClient)
        {
            _requestsStorage = requestsStorage;

            _usersClient = usersClient;
            _roomsClient = roomsClient;
            _monstersClient = monstersClient;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TIME_DELAY);

                Console.WriteLine("Execute TryAttackMonster!");
                await TryAttackMonster();
            }
        }

        private async Task TryAttackMonster()
        {
            try
            {
                if (!await _usersClient.HealthCheck())
                {
                    Console.WriteLine("User service is unavailable");
                    return;
                }
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine("User service is unavailable");
                return;
            }

            try
            {
                if (!await _monstersClient.HealthCheck())
                {
                    Console.WriteLine("Monster service is unavailable");
                    return;
                }
            }
            catch(BrokenCircuitException)
            {
                Console.WriteLine("Monster service is unavailable");
                return;
            }

            while (_requestsStorage.AttackMonsterRequestQueue.Count != 0)
            {
                if (_requestsStorage.AttackMonsterRequestQueue.TryDequeue(out var attackMonsterInfo))
                {
                    AttackMonsterInfo newAttackMonsterInfo = new AttackMonsterInfo() { UserId = attackMonsterInfo.UserId };

                    try
                    {
                        Player player = await _usersClient.GetPlayerById(attackMonsterInfo.UserId);

                        if (player.CurrentRoomId != null)
                        {
                            Room room = await _roomsClient.GetRoomById((int)player.CurrentRoomId);
                            if (room.MonsterId != null)
                            {
                                // Attack monster by player
                                bool isDead = await _monstersClient.PatchAttackMonster((int)room.MonsterId);
                                newAttackMonsterInfo.isMonsterHPUpdated = true;

                                // If it didn't died it attack player too
                                player = await _usersClient.PatchHpLoss(attackMonsterInfo.UserId);
                                newAttackMonsterInfo.isUserHPUpdated = true;
                            }
                        }
                    }
                    catch (BrokenCircuitException)
                    {
                        _requestsStorage.AttackMonsterRequestQueue.Enqueue(newAttackMonsterInfo);
                    }
                }
            }
        }
    }
}
