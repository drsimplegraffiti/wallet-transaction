using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWalletAsync([FromBody] CreateWalletOthersDto createWalletDto)
        {
            var response = await _userService.CreateWalletAsync(createWalletDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferFundDto transferDto)
        {
            var response = await _userService.TransferAsync(transferDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("switch-environment")]
        public async Task<IActionResult> SwitchEnvironmentAsync([FromBody] SwitchEnvironmentDto switchEnvironmentDto)
        {
            var response = await _userService.SwitchEnvironmentAsync(switchEnvironmentDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("show-balance")]
        public async Task<IActionResult> ShowBalanceAsync()
        {
            var response = await _userService.ShowBalanceAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("banks")]
        [SkipTokenValidation]
        public async Task<IActionResult> GetBanksAsync()
        {
            var response = await _userService.GetBanksAsync();
            return Ok(response);
        }

    }
}
