
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

        Task<string> GetBanksAsync();
}

}