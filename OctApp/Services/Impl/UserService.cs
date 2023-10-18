using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Data;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Models;
using OctApp.Services.Interface;
using OctApp.Utils.Interface;

namespace OctApp.Services.Impl
{
    public class UserService : IUserService
    {


        private readonly IWalletService _walletService;
        private readonly DataContext _dbContext;
        private readonly ITokenService _tokenService;

        private readonly ILogger<UserService> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IWalletService walletService, DataContext dbContext, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _walletService = walletService;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ApiResponse<dynamic>> CreateWalletAsync(CreateWalletOthersDto createWalletDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var principal = _httpContextAccessor.HttpContext!.User;
                    if (principal == null)
                    {
                        return new ApiResponse<dynamic>(401, message: "Unauthorized");
                    }
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                        _logger.LogInformation("userIdClaim:********* " + userIdClaim);

                    if (userIdClaim == null)
                    {
                        return new ApiResponse<dynamic>(401, message: "Unauthorized");
                    }

                    //create a wallet for the user
                    var (walletAddress, publicKey, privateKey) = _walletService.GenerateWallet();
                    var wallet = new Wallet
                    {
                        Name = createWalletDto.Name,
                        Symbol = createWalletDto.Symbol,
                        Address = walletAddress,
                        PublicKey = publicKey,
                        PrivateKey = privateKey,
                        UserId = int.Parse(userIdClaim),
                        Balance = 0,
                        AppEnvironmentId = 1
                    };

                    _dbContext.Wallets.Add(wallet);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return new ApiResponse<dynamic>(200, data: new
                    {
                        walletAddress,
                        publicKey,
                        privateKey
                    });

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new ApiResponse<dynamic>(500, message: e.Message);
                }
            }
        }
    }
}