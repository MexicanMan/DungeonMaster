using Gateway.Configs;
using Gateway.Exceptions;
using Gateway.Helpers;
using Gateway.Models;
using Gateway.Services.Clients;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GameService : IGameService
    {
        private readonly IUsersClient _usersClient;
        private readonly IRoomsClient _roomsClient;
        private readonly IMonstersClient _monstersClient;

        private readonly GameLogicSettings _gameLogicSettings;
        private Random _random;

        public GameService(IUsersClient usersClient, IRoomsClient roomsClient, IMonstersClient monstersClient,
            IOptions<AppSettings> appSettingsAccessor)
        {
            _usersClient = usersClient;
            _roomsClient = roomsClient;
            _monstersClient = monstersClient;

            _gameLogicSettings = appSettingsAccessor.Value.GameLogic;
            _random = new Random();
        }

        public async Task<Player> GetPlayer(string userId)
        {
            Player player = await _usersClient.GetPlayerById(userId);

            return player;
        }

        public async Task<GameData> EnterGame(string userId)
        {
            Player player = await GetPlayer(userId);

            Room room;
            if (player.CurrentRoomId == null)
            {
                // If player new - create room for him and make him it's discoverer
                room = await CreateNewRoom(userId, Direction.None, null);

                // Update player CurrentRoomId
                player = await _usersClient.PatchRoomChange(userId, room.RoomId);
            }
            else
            {
                // If player old - return him to his room
                room = await _roomsClient.GetRoomById((int) player.CurrentRoomId);
            }

            GameData data = new GameData() { 
                Room = room,
                Player = player,
                Monster = null
            };
            if(room.MonsterId != null)
            {
                data.Monster = await _monstersClient.GetMonsterById((int) room.MonsterId);
            }

            return data;
        }

        public async Task<Room> CreateNewRoom(string discovererId, Direction fromDir, int? fromRoomId)
        {
            // Initial create of room
            RoomWrapper wrapper = await _roomsClient.PostCreateRoom(discovererId, fromDir, fromRoomId);
            Room room = wrapper.Room;

            //Check whether monster needed to be created
            if (wrapper.IsMonsterNeeded)
            {
                // Create monster and recieve it's id
                int monsterId = await _monstersClient.PostCreateMonster();
                // Put monsterId at room
                room = await _roomsClient.PatchPutMonster(room.RoomId, monsterId);
            }

            return room;
        }

        public async Task<GameData> PickupTreasure(string userId)
        {
            Player player = await GetPlayer(userId);

            if (player.CurrentRoomId != null)
            {
                Room room = await _roomsClient.GetRoomById((int) player.CurrentRoomId);
                if (room.MonsterId == null)
                {
                    // Pickup treasure in the room
                    room = await _roomsClient.PatchPickupTreasure((int) player.CurrentRoomId);
                    // And give it to the player
                    player = await _usersClient.PatchPickupTreasure(userId);

                    return new GameData() { Player = player, Room = room, Monster = null };
                }
                else
                    throw new PermissionsDeniedException("Monster still alive!");
            }
            else
                throw new NullReferenceException("Player has no room!");
        }

        public async Task<GameData> AttackMonster(string userId)
        {
            Player player = await GetPlayer(userId);

            if (player.CurrentRoomId != null)
            {
                Room room = await _roomsClient.GetRoomById((int) player.CurrentRoomId);
                if (room.MonsterId != null)
                {
                    // Attack monster by player
                    bool isDead = await _monstersClient.PatchAttackMonster((int) room.MonsterId);
                    Monster monster = null;
                    if (!isDead)
                    {
                        // If it didn't died it attack player too
                        player = await _usersClient.PatchHpLoss(userId);
                        monster = await _monstersClient.GetMonsterById((int) room.MonsterId);

                        // If player died
                        if (player.CurrentHP <= 0)
                        {
                            // ?
                        }
                    }
                    else
                    {
                        // If monster died clear it from everywhere
                        await _monstersClient.DeleteMonster((int) room.MonsterId);
                        room = await _roomsClient.DeleteMonster(room.RoomId);
                    }

                    return new GameData() { Player = player, Room = room, Monster = monster };
                }
                else
                    throw new NullReferenceException("No alive monsters in the room!");
            }
            else
                throw new NullReferenceException("Player has no room!");
        }

        public async Task<GameData> Move(string userId, Direction to)
        {
            Player player = await GetPlayer(userId);

            if (player.CurrentRoomId != null)
            {
                Room room = await _roomsClient.GetRoomById((int) player.CurrentRoomId);

                int toRoomId = -1;
                bool isDoorClosed = true;
                switch (to)
                {
                    case Direction.East:
                        if (room.EastRoom)  // If door exists
                        {
                            if (room.EastRoomId != null)  // If door already opened 
                            {
                                isDoorClosed = false;
                                toRoomId = (int) room.EastRoomId;
                            }
                        }
                        else
                            throw new WrongDirectionException("No such door!");
                        break;
                    case Direction.West:
                        if (room.WestRoom)
                        {
                            if (room.WestRoomId != null)
                            {
                                isDoorClosed = false;
                                toRoomId = (int) room.WestRoomId;
                            }
                        }
                        else
                            throw new WrongDirectionException("No such door!");
                        break;
                    case Direction.North:
                        if (room.NorthRoom)
                        {
                            if (room.NorthRoomId != null)
                            {
                                isDoorClosed = false;
                                toRoomId = (int) room.NorthRoomId;
                            }
                        }
                        else
                            throw new WrongDirectionException("No such door!");
                        break;
                    case Direction.South:
                        if (room.SouthRoom)
                        {
                            if (room.SouthRoomId != null)
                            {
                                isDoorClosed = false;
                                toRoomId = (int) room.SouthRoomId;
                            }
                        }
                        else
                            throw new WrongDirectionException("No such door!");
                        break;
                    default:
                        throw new WrongDirectionException("Wrong direction!");
                }

                // Open door if it wasn't before and create or get random fitting room
                if (isDoorClosed)
                {
                    if (room.MonsterId == null)
                    {
                        toRoomId = await MoveInitRoom(userId, to, room.RoomId);
                        room = await _roomsClient.PatchOpenDoor(room.RoomId, to, toRoomId);
                    }
                    else
                        throw new PermissionsDeniedException("Kill monster to open closed door!");
                }

                // Move to the next room and return it
                player = await _usersClient.PatchRoomChange(userId, toRoomId);
                room = await _roomsClient.GetRoomById(toRoomId);
                GameData data = new GameData()
                {
                    Room = room,
                    Player = player,
                    Monster = null
                };
                if (room.MonsterId != null)
                {
                    data.Monster = await _monstersClient.GetMonsterById((int) room.MonsterId);
                }

                return data;
            }
            else
                throw new NullReferenceException("Player has no current room!");
        }

        public async Task<IEnumerable<LeaderboardUser>> GetLeaderboard(int page)
        {
            return await _usersClient.GetLeaderboard(page);
        }



        private async Task<int> MoveInitRoom(string userId, Direction to, int currentRoomId)
        {
            bool isOldRandomRoom = _random.NextDouble() * 100 < _gameLogicSettings.ChanceTakeRandomRoom ? true : false;

            Room room;
            // If decided to take old random room
            if (isOldRandomRoom)
            {
                room = await _roomsClient.GetRandomRoom(to);

                // Open door in the obtained room
                Direction newRoomDoorDir;
                switch (to)
                {
                    case Direction.East:
                        newRoomDoorDir = Direction.West;
                        break;
                    case Direction.West:
                        newRoomDoorDir = Direction.East;
                        break;
                    case Direction.North:
                        newRoomDoorDir = Direction.South;
                        break;
                    case Direction.South:
                        newRoomDoorDir = Direction.North;
                        break;
                    default:
                        throw new WrongDirectionException("Wrong direction!");
                }
                room = await _roomsClient.PatchOpenDoor(room.RoomId, newRoomDoorDir, currentRoomId);
            }
            else  // If decided create new room or wasn't found any fitting random room
                room = await CreateNewRoom(userId, to, currentRoomId);

            return room.RoomId;
        }
    }
}
