using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.API.Exceptions;
using User.API.Models;
using User.API.Services;

namespace User.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "Player")]
        [HttpGet("token")]
        public async Task<IActionResult> CheckToken()
        {
            return Ok("Token correct!");
        }

        [HttpPost("authserver")]
        public async Task<IActionResult> AuthUser([FromBody] AuthUserModel auth)
        {
            if (auth.AppId == 1 && auth.AppSecret == "user_secret")
            {
                //var token = ;
                return Ok();
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] Login data)
        {
            try
            {
                var jwt = await _usersService.Auth(data.Username, data.Password);

                return Ok(jwt);
            }
            catch (AuthException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register([FromBody] Login data)
        {
            try
            {
                var user = await _usersService.RegisterUser(data.Username, data.Password);

                return Ok(user);
            }
            catch (RegisterException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            try
            {
                var user = await _usersService.GetUserById(id);

                return Ok(user);
            }
            catch (NoSuchUserException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int page = 0, int pageSize = 10)
        {
            var leaderboardPage = await _usersService.GetLeaderboard(page, pageSize);

            return Ok(leaderboardPage);
        }

        [HttpPatch("{id}/treasure")]
        public async Task<IActionResult> TreasurePickup(string id)
        {
            try
            {
                var user = await _usersService.TreasurePickup(id);

                return Ok(user);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchUserException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpPatch("{id}/hp")]
        public async Task<IActionResult> HPLoss(string id)
        {
            try
            {
                var user = await _usersService.HPLoss(id);

                return Ok(user);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchUserException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (AlreadyDeadException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpPatch("{id}/room")]
        public async Task<IActionResult> RoomChange(string id, [FromBody] RoomIdWrapper roomId)
        {
            try
            {
                var user = await _usersService.RoomChange(id, roomId.RoomId);

                return Ok(user);
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchUserException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet("failing")]
        public async Task<IActionResult> FailingGet()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ooops");
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            return Ok();
        }
    }
}
