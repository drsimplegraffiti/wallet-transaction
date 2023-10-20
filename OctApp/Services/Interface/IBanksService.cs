using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Dto.Request;
using OctApp.Dto.Response;

namespace OctApp.Services.Interface
{
    public interface IBanksService
    {
        Task<ApiResponse<dynamic>> GetBanksAsync();
        Task<ApiResponse<dynamic>> CreateBankAsync(BankDto bankDto);
    }
}