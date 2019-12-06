using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monster.API.Configs;
using Monster.API.Exceptions;
using Monster.API.Models;
using Monster.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Controllers
{
    [Route("api/monsters")]
    [Authorize(Policy = "Gateway")]
    [ApiController]
    public class MonstersController : ControllerBase
    {
        private readonly IMonstersService _monstersService;
        private readonly TokenStorage _tokenStorage;

        public MonstersController(IMonstersService monstersService, TokenStorage tokenStorage)
        {
            _monstersService = monstersService ?? throw new ArgumentNullException(nameof(monstersService));
            _tokenStorage = tokenStorage;
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthMonster([FromBody] AuthMonsterModel auth)
        {
            if (auth.AppId == 2 && auth.AppSecret == "monster_secret")
            {
                int expiration = 3600;
                var token = $"{(Int32) (DateTime.UtcNow.AddSeconds(expiration).Subtract(new DateTime(1970, 1, 1))).TotalSeconds}.{Guid.NewGuid().ToString()}";
                _tokenStorage.activeTokens.Add(token);
                return Ok(new TokenModel() { Token = token, Exp_in = expiration });
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRandomMonster()
        {
            try
            {
                int monsterId = await _monstersService.CreateRandomMonster();

                return Ok(monsterId);
            }
            catch(DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonster(int id)
        {
            var monster = await _monstersService.GetMonster(id);
            if (monster != null)
                return Ok(monster);
            else
                return StatusCode(StatusCodes.Status400BadRequest, "No such monster!");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> AttackMonster(int id)
        {
            try
            {
                bool isDead = await _monstersService.AttackMonster(id);

                return Ok(isDead);
            }
            catch(DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch(NoSuchMonsterException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMonster(int id)
        {
            try
            {
                await _monstersService.RemoveMonster(id);

                return Ok();
            }
            catch (DatabaseException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (NoSuchMonsterException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
    }
}
