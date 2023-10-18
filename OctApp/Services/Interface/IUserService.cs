using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Dto.Request;
using OctApp.Dto.Response;

namespace OctApp.Services.Interface
{
    public interface IUserService
    {
        Task<ApiResponse<dynamic>> CreateWalletAsync(CreateWalletOthersDto createWalletDto);
}

    public class CreateWalletOthersDto
    {
        public string Name { get; set; } = "Binance Naira Balance";

        public string Symbol { get; set; } = "BNB";
        
    }
}