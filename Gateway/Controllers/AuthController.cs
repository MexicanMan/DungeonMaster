using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Exceptions;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Controllers
{
    [Route("api/auth")]
    [EnableCors]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] Login data)
        {
            try
            {
                var resp = await _authService.Login(data.Username, data.Password);

                return Ok(resp);
            }
            catch (AuthException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {


            return Ok();
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register([FromBody] Login data)
        {
            try
            {
                var authResp = await _authService.RegisterAndLogin(data.Username, data.Password);

                return Ok(authResp);
            }
            catch(AuthException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (RegistrationException e)
            {
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
            catch (InternalException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse() { Error = e.Message });
            }
        }
    }
}
