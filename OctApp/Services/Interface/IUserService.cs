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

        Task<ApiResponse<dynamic>> TransferAsync(TransferFundDto transferDto);

        Task<ApiResponse<dynamic>> SwitchEnvironmentAsync(SwitchEnvironmentDto switchEnvironmentDto);

        Task<ApiResponse<dynamic>> ShowBalanceAsync();
}

}