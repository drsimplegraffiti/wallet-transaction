using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctApp.Dto.Request;
using OctApp.Services.Interface;

namespace OctApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BankController: ControllerBase
    {

        private readonly ILogger<BankController> _logger;
        private readonly IBanksService _banksService;

        public BankController(IBanksService banksService, ILogger<BankController> logger)
        {
            _banksService = banksService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBanksAsync()
        {
            try
            {
                var response = await _banksService.GetBanksAsync();
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while retrieving banks");
                return StatusCode(500, "Error occurred while retrieving banks");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateBankAsync([FromBody] BankDto bankDto)
        {
            try
            {
                var response = await _banksService.CreateBankAsync(bankDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating bank");
                return StatusCode(500, "Error occurred while creating bank");
            }
        }


    }
}