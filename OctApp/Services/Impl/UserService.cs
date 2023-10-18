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


        public UserService(
            IWalletService walletService, 
            DataContext dbContext, ITokenService tokenService, 
            ILogger<UserService> logger, 
            IHttpContextAccessor httpContextAccessor = null!
            )
        {
            _walletService = walletService;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
                       return new ApiResponse<dynamic>{Success = false, StatusCode = 401, Message = "Unauthorized"};
                    }
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                    _logger.LogInformation("userIdClaim:********* " + userIdClaim);

                    if (userIdClaim == null)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 401, Message = "Unauthorized"};
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


                    return new ApiResponse<dynamic>{Success = true, StatusCode = 200, Data = new
                    {
                        walletAddress,
                        publicKey,
                        privateKey
                    }};

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new ApiResponse<dynamic>{Success = false, StatusCode = 500, Message = e.Message};
                }
            }
        }
        public async Task<ApiResponse<dynamic>> TransferAsync(TransferFundDto transferDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var principal = _httpContextAccessor.HttpContext?.User;
                    if (principal == null)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 401, Message = "Unauthorized"};
                    }

                    var userIdClaim = principal.FindFirst("userId")?.Value;
                    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 401, Message = "Unauthorized"};
                    }

                    var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 404, Message = "User not found"};
                    }

                    var wallet = await _dbContext.Wallets.Where(w => w.Address == transferDto.Sender).FirstOrDefaultAsync();
                    if (wallet == null)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 404, Message = "Sender wallet not found"};
                    }

                    if (wallet.UserId != userId)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 401, Message = "Unauthorized"};
                    }


                    if (wallet.Balance < transferDto.Amount)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 400, Message = "Insufficient balance"};
                    }

                    var recipientWallet = _dbContext.Wallets.FirstOrDefault(w => w.Address == transferDto.Recipient);
                    if (recipientWallet == null)
                    {
                        return new ApiResponse<dynamic>{Success = false, StatusCode = 404, Message = "Recipient wallet not found"};
                    }

                    var newBalance = wallet.Balance - transferDto.Amount;

                    wallet.Balance = newBalance;
                    _dbContext.Wallets.Update(wallet);
                    await _dbContext.SaveChangesAsync();

                    recipientWallet.Balance += transferDto.Amount;
                    _dbContext.Wallets.Update(recipientWallet);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                   
                    return new ApiResponse<dynamic>{Success = true, StatusCode = 200, Data = new
                    {
                        walletAddress = wallet.Address,
                        balance = wallet.Balance
                    }};
                }

                catch (Exception e)
                {
                    transaction.Rollback();
                    return new ApiResponse<dynamic>{Success = false, StatusCode = 500, Message = e.Message};
                }
            }
        }

    }
}