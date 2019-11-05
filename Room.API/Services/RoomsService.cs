using Microsoft.Extensions.Options;
using Room.API.Configs;
using Room.API.Exceptions;
using Room.API.Models;
using Room.API.Models.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Services
{
    public class RoomsService : IRoomsService
    {
        private const int MAX_DOORS = 4;

        private readonly RoomContext _roomContext;
        private readonly AppSettings _appSettings;
        private readonly Random _random;

        public RoomsService(RoomContext context, IOptions<AppSettings> appSettingsAccessor)
        {
            _roomContext = context;
            _appSettings = appSettingsAccessor.Value;

            _random = new Random();
        }

        public async Task<RoomAnswer> CreateRoom(string discovererId, Direction discovererDirection, 
            int? discovererFromRoomId)
        {
            bool isTreasure = _random.NextDouble() * 100 < _appSettings.TreasureChance ? true : false;
            bool isMonster = _random.NextDouble() * 100 < _appSettings.MonsterChance ? true : false;

            // Randoming doors count
            int doorCount = 2;
            double prob = _random.NextDouble() * 100;
            if (prob < _appSettings.ThirdDoorChance)
            {
                doorCount = 3;
                prob = _random.NextDouble() * 100;
                if (prob < _appSettings.FourthDoorChance)
                    doorCount = 4;
            }

            bool[] directions = new bool[4];
            int?[] roomIds = new int?[4];

            // Deal with directions first
            // Define if player come from somewhere
            int currentDoorCount = 1;
            switch (discovererDirection)
            {
                case Direction.East:
                    directions[1] = true;
                    roomIds[1] = discovererFromRoomId;
                    break;
                case Direction.West:
                    directions[0] = true;
                    roomIds[0] = discovererFromRoomId;
                    break;
                case Direction.North:
                    directions[3] = true;
                    roomIds[3] = discovererFromRoomId;
                    break;
                case Direction.South:
                    directions[2] = true;
                    roomIds[2] = discovererFromRoomId;
                    break;
                default:  // If player have just been created
                    currentDoorCount = 0;
                    break;
            }
            //  Randoming doors
            while (currentDoorCount < doorCount)
            {
                int doorNum = _random.Next(0, MAX_DOORS - currentDoorCount);
                for (int i = 0, currentIterRoom = 0; i < directions.Length && currentIterRoom <= doorNum; i++)
                {
                    if (directions[i] != true)
                    {
                        if (currentIterRoom == doorNum)
                        {
                            directions[i] = true;
                            currentDoorCount++;
                        }

                        currentIterRoom++;
                    }
                }
            }

            RoomModel room = new RoomModel
            {
                Treasure = isTreasure,
                MonsterId = null,
                EastRoom = directions[0],
                WestRoom = directions[1],
                NorthRoom = directions[2],
                SouthRoom = directions[3],
                EastRoomId = roomIds[0],
                WestRoomId = roomIds[1],
                NorthRoomId= roomIds[2],
                SouthRoomId = roomIds[3],
                DiscovererId = discovererId
            };

            try
            {
                _roomContext.Rooms.Add(room);
                await _roomContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.Message, e);
            }

            return new RoomAnswer
            {
                Room = room,
                IsMonsterNeeded = isMonster
            };
        }

        public async Task<RoomModel> MosterPut(int roomId, int monsterId)
        {
            RoomModel room = _roomContext.Rooms.SingleOrDefault(r => r.RoomId == roomId);

            if (room != null)
            {
                room.MonsterId = monsterId;

                try
                {
                    _roomContext.Rooms.Update(room);
                    await _roomContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.Message, e);
                }
            }
            else
            {
                throw new NoSuchRoomException("No such room!");
            }

            return room;
        }

        public async Task<RoomModel> GetRandom(Direction fromDirection)
        {
            // Define what direction in room should be closed
            IEnumerable<RoomModel> rooms;
            switch (fromDirection)
            {
                case Direction.East:
                    rooms = _roomContext.Rooms.Where(r => r.WestRoom && r.WestRoomId == null);
                    break;
                case Direction.West:
                    rooms = _roomContext.Rooms.Where(r => r.EastRoom && r.EastRoomId == null);
                    break;
                case Direction.North:
                    rooms = _roomContext.Rooms.Where(r => r.SouthRoom && r.SouthRoomId == null);
                    break;
                case Direction.South:
                    rooms = _roomContext.Rooms.Where(r => r.NorthRoom && r.NorthRoomId == null);
                    break;
                default:
                    throw new WrongDirectionException("Wrong direction!");
            }

            RoomModel room = null;
            if (rooms.Count() > 0)
                room = rooms.ElementAt(_random.Next(0, rooms.Count()));

            return room;
        }

        public async Task<RoomModel> Get(int id)
        {
            RoomModel room = _roomContext.Rooms.SingleOrDefault(r => r.RoomId == id);

            return room;
        }

        public async Task<RoomModel> DoorOpen(int thisRoomId, Direction dir, int toRoomId)
        {
            RoomModel room = _roomContext.Rooms.SingleOrDefault(r => r.RoomId == thisRoomId);

            if (room != null)
            {
                switch (dir)
                {
                    case Direction.East:
                        if (room.EastRoom && room.EastRoomId == null)
                            room.EastRoomId = toRoomId;
                        else
                            throw new WrongDirectionException("No such door or door is already opened!");
                        break;
                    case Direction.West:
                        if (room.WestRoom && room.WestRoomId == null)
                            room.WestRoomId = toRoomId;
                        else
                            throw new WrongDirectionException("No such door or door is already opened!");
                        break;
                    case Direction.North:
                        if (room.NorthRoom && room.NorthRoomId == null)
                            room.NorthRoomId = toRoomId;
                        else
                            throw new WrongDirectionException("No such door or door is already opened!");
                        break;
                    case Direction.South:
                        if (room.SouthRoom && room.SouthRoomId == null)
                            room.SouthRoomId = toRoomId;
                        else
                            throw new WrongDirectionException("No such door or door is already opened!");
                        break;
                    default:
                        throw new WrongDirectionException("Wrong direction!");
                }

                try
                {
                    _roomContext.Rooms.Update(room);
                    await _roomContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.Message, e);
                }
            }
            else
                throw new NoSuchRoomException("No such room!");

            return room;
        }

        public async Task<RoomModel> TreasurePickup(int id)
        {
            RoomModel room = _roomContext.Rooms.SingleOrDefault(r => r.RoomId == id);

            if (room != null)
            {
                if (room.Treasure)
                {
                    room.Treasure = false;

                    try
                    {
                        _roomContext.Rooms.Update(room);
                        await _roomContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseException(e.Message, e);
                    }
                }
                else
                    throw new NoTreasureException($"No treasure in room {id}!");
            }
            else
                throw new NoSuchRoomException("No such room!");

            return room;
        }

        public async Task<RoomModel> MonsterDeletion(int id)
        {
            RoomModel room = _roomContext.Rooms.SingleOrDefault(r => r.RoomId == id);

            if (room != null)
            {
                if (room.MonsterId != null)
                {
                    room.MonsterId = null;

                    try
                    {
                        _roomContext.Rooms.Update(room);
                        await _roomContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseException(e.Message, e);
                    }
                }
                else
                    throw new NoMonsterException($"No monster in room {id}!");
            }
            else
                throw new NoSuchRoomException("No such room!");

            return room;
        }
    }
}
