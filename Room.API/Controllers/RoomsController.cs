using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Room.API.Exceptions;
using Room.API.Models;
using Room.API.Models.TransferModels;
using Room.API.Services;

namespace Room.API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService ?? throw new ArgumentNullException(nameof(roomsService));
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthRoom([FromBody] AuthRoomModel auth)
        {
            if (auth.AppId == 3 && auth.AppSecret == "room_secret")
            {
                //var token = ;
                return Ok();
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Discoverer body)
        {
            try
            {
                var room = await _roomsService.CreateRoom(body.DiscovererId, body.FromDirection, body.FromRoomId);

                return Ok(room);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("{id}/monster")]
        public async Task<IActionResult> MonsterPut(int id, [FromBody] Monster body)
        {
            try
            {
                var room = await _roomsService.MosterPut(id, body.MonsterId);

                return Ok(room);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchRoomException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRandom([FromQuery] string from)
        {
            try
            {
                from = char.ToUpper(from[0]) + from.Substring(1);
                bool res = Direction.TryParse(from, out Direction dir);
                if (res)
                {
                    var room = await _roomsService.GetRandom(dir);

                    if (room != null)
                        return Ok(room);
                    else
                        return NoContent();
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Wrong direction type!");
            }
            catch (WrongDirectionException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var room = await _roomsService.Get(id);
            if (room != null)
                return Ok(room);
            else
                return StatusCode(StatusCodes.Status400BadRequest, "No such room!");
        }

        [HttpPatch("{id}/door")]
        public async Task<IActionResult> OpenDoor(int id, [FromBody] DoorOpen body)
        {
            try
            {
                 var room = await _roomsService.DoorOpen(id, body.Direction, body.ToRoomId);

                return Ok(room);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchRoomException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpPatch("{id}/treasure")]
        public async Task<IActionResult> TreasurePickup(int id)
        {
            try
            {
                var room = await _roomsService.TreasurePickup(id);

                return Ok(room);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchRoomException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (NoTreasureException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpDelete("{id}/monster")]
        public async Task<IActionResult> MonsterDeletion(int id)
        {
            try
            {
                var room = await _roomsService.MonsterDeletion(id);

                return Ok(room);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchRoomException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (NoMonsterException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpPatch("{id}/update")]
        public async Task<IActionResult> RoomUpdate(int id, [FromBody] RoomModel room)
        {
            try
            {
                var newRoom = await _roomsService.RoomUpdate(id, room);

                return Ok(newRoom);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchRoomException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
    }
}
