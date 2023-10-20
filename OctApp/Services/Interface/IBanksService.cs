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

        Task<string> LoginAsync(LoginDto loginDto);
    }

    public class UserLoginResponseDto
    {

       public string Token { get; set; } = string.Empty;
       public string RefreshToken { get; set; } = string.Empty;

    }
}