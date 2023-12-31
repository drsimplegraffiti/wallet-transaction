using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Services.Interface;

namespace OctApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var response = await _authService.RefreshTokenAsync(refreshTokenDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto createUserDto)
        {
           try
           {
             var response = await _authService.CreateUserAsync(createUserDto);
             if(response.Success)
             {
                 return Ok(response);
             }
                return StatusCode(response.StatusCode, response);
           }
           catch (Exception e)
           {
            return BadRequest(e.Message);
           }
        }
    }
}