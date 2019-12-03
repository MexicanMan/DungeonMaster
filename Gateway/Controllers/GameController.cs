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
using Polly.CircuitBreaker;

namespace Gateway.Controllers
{
    [Route("api/game")]
    [Authorize(Policy = "Player")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                Player player = await _gameService.GetPlayer(userId);

                return Ok(player);
            }
            catch (WrongInputDataException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }

        [HttpGet("room")]
        public async Task<IActionResult> EnterGame()
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                GameData data = await _gameService.EnterGame(userId);

                return Ok(data);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
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

        [HttpPatch("treasure")]
        public async Task<IActionResult> PickUpTreasure()  
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                GameData data = await _gameService.PickupTreasure(userId);

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
            catch (RollbackException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
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
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }

        [HttpPatch("room")]
        public async Task<IActionResult> Move([FromBody] DirectionWrapper to)
        {
            string userId = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                GameData data = await _gameService.Move(userId, to.ToDir);

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
            catch (WrongDirectionException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (PermissionsDeniedException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse() { Error = e.Message });
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int page)
        {
            try
            {
                IEnumerable<LeaderboardUser> leaderboardPage = await _gameService.GetLeaderboard(page);

                return Ok(new LeaderboardModel() { Players = leaderboardPage });
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse()
                {
                    Error = "Some services are not feeling " +
                    "good. Try later!"
                });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }
    }
}