using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctApp.Dto.Request;
using OctApp.Services.Interface;
using OctApp.Utils;

namespace OctApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(TokenValidationFilterAttribute))] // Apply the token validation filter

    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWalletAsync([FromBody] CreateWalletOthersDto createWalletDto)
        {
            var response = await _userService.CreateWalletAsync(createWalletDto);
            return Ok(response);
        }
    }
}
