using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    public class MonstersController : ControllerBase
    {
        private readonly IMonstersService _monstersService;

        public MonstersController(IMonstersService monstersService)
        {
            _monstersService = monstersService ?? throw new ArgumentNullException(nameof(monstersService));
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

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            return Ok();
        }
    }
}
