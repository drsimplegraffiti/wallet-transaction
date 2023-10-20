using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OctApp.Data;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Models;
using OctApp.Services.Interface;
using RestSharp;

namespace OctApp.Services.Impl
{
    public class BankService : IBanksService
    {

        private readonly DataContext _context;
        private readonly ILogger<BankService> _logger;

        public BankService(DataContext context, ILogger<BankService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<dynamic>> CreateBankAsync(BankDto bankDto)
        {
            var bank = new Bank
            {
                BankName = bankDto.BankName,
                BankCode = bankDto.BankCode,
                BankId = bankDto.BankId
            };

            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();

            return ApiResponse<dynamic>.OkResponse(bank, "Bank created successfully");
        }

        public async Task<ApiResponse<dynamic>> GetBanksAsync()
        {
            var banks = await _context.Banks.ToListAsync();
            var response = new { banks };
            return ApiResponse<dynamic>.OkResponse(response, "Banks retrieved successfully");
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
              try
              {
                  var baseurl = "http://localhost:5037/";
               var client = new RestClient(baseurl);
                var request = new RestRequest("api/Auth/login", Method.Post);
                request.AddHeader("Content-Type", "application/json");

                request.AddJsonBody(loginDto);
                var response =await client.ExecuteAsync<UserLoginResponseDto>(request);
                _logger.LogInformation(response.StatusCode.ToString());
                _logger.LogInformation(response.Content);
             
                return response.Content ?? throw new Exception("Error occurred while logging in");
              }
              catch (Exception ex)
              {
                return ex.Message;
                
              }

        }
    }
}