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

namespace OctApp.Services.Impl
{
    public class BankService : IBanksService
    {

        private readonly DataContext _context;

        public BankService(DataContext context)
        {
            _context = context;
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
    }
}