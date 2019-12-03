using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Exceptions;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/lab4")]
    [Authorize(Policy = "Player")]
    [ApiController]
    public class Lab4Controller : ControllerBase
    {
        private readonly ILab4Service _gameService;

        public Lab4Controller(ILab4Service gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [HttpGet("degr/room")]
        public async Task<IActionResult> DegradatedRoom()
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                GameData data = await _gameService.GetDegrRoom(userId);

                return Ok(data);
            }
            catch (WrongInputDataException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }

        [HttpPatch("monster")]
        public async Task<IActionResult> AttackMonster()
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                GameData data = await _gameService.AttackMonster(userId);

                return Ok(data);
            }
            catch (WrongInputDataException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse() { Error = e.Message });
            }
            catch (PermissionsDeniedException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse() { Error = e.Message });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }
    }
}