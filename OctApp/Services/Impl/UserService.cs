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
using OctApp.Utils;
using OctApp.Utils.Interface;

namespace OctApp.Services.Impl
{
    public class UserService : IUserService
    {


        private readonly IWalletService _walletService;
        private readonly DataContext _dbContext;

        private readonly IKeyService _keyService;
        private readonly ITokenService _tokenService;

        private readonly ILogger<UserService> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(
            IWalletService walletService,
            DataContext dbContext, ITokenService tokenService,
            ILogger<UserService> logger,
            IKeyService keyService,
            IHttpContextAccessor httpContextAccessor = null!
            )
        {
            _walletService = walletService;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _logger = logger;
            _keyService = keyService;
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
                        return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                    }
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                    _logger.LogInformation("userIdClaim:********* " + userIdClaim);

                    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                    {
                        return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                    }

                    // Generate test wallet keys
                    string testPrivateKey = _keyService.GeneratePrivateKey();
                    string testPublicKey = _keyService.GeneratePublicKey();

                    // Generate live wallet keys
                    string livePrivateKey = _keyService.GeneratePrivateKey();
                    string livePublicKey = _keyService.GeneratePublicKey();


                    var wallet = new Wallet
                    {
                        AccountNumber = AccountNumberGenerator.GenerateAccountNumber(),
                        TestPrivateKey = "Test" + "-" + testPrivateKey,
                        TestPublicKey = "Test" + "-" + testPublicKey,
                        LivePrivateKey = "Live" + "-" + livePrivateKey,
                        LivePublicKey = "Live" + "-" + livePublicKey,
                        TestBalance = 1000m, // Set test balance
                        LiveBalance = 0m,
                        AccountName = $"Binance Naira Balance {userId}",
                        AppEnvironmentId = 2, // Assume 2 represents the test environment
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _dbContext.Wallets.Add(wallet);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return new ApiResponse<dynamic>
                    {
                        Success = true,
                        StatusCode = 200,
                        Data = new
                        {


                        }
                    };


                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 500, Message = e.Message };
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
                return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
            }

            var userIdClaim = principal.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "User not found" };
            }

            // Check the user's environment
            var appEnvironment = user.AppEnvironmentId;
            var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId && w.AppEnvironmentId == appEnvironment);

            if (wallet == null)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "Wallet not found" };
            }

            // ensure you cant transfer money to 
            if (transferDto.RecipientAccountNumber == wallet.AccountNumber)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 400, Message = "You can't transfer money to yourself" };
            }

            // Check if the sender has enough balance
            decimal senderBalance = appEnvironment == 1 ? wallet.LiveBalance : wallet.TestBalance;

            if (senderBalance < transferDto.Amount)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 400, Message = "Insufficient balance" };
            }

            // Check if the recipient exists
            var recipient = _dbContext.Wallets.FirstOrDefault(u => u.AccountNumber == transferDto.RecipientAccountNumber);
            if (recipient == null)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "Recipient not found" };
            }

            // Check if the recipient has a wallet
            var recipientWallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == recipient.Id && w.AppEnvironmentId == appEnvironment);

            if (recipientWallet == null)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "Recipient wallet not found" };
            }

            // Update the sender's balance
            if (appEnvironment == 1)
            {
                wallet.LiveBalance -= transferDto.Amount;
            }
            else if (appEnvironment == 2)
            {
                wallet.TestBalance -= transferDto.Amount;
            }

            _dbContext.Wallets.Update(wallet);

            // Update the recipient's balance
            if (appEnvironment == 1)
            {
                recipientWallet.LiveBalance += transferDto.Amount;
            }
            else if (appEnvironment == 2)
            {
                recipientWallet.TestBalance += transferDto.Amount;
            }

            _dbContext.Wallets.Update(recipientWallet);

            // Create a transaction record
            var transactionRecord = new Transaction
            {
                // Sender = user.Email, use sender account number
                Sender = wallet.AccountNumber,
                Recipient = recipient.AccountNumber,
                Amount = transferDto.Amount,
                AppEnvironmentId = appEnvironment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Transactions.Add(transactionRecord);
            await _dbContext.SaveChangesAsync();

            transaction.Commit();

            return new ApiResponse<dynamic>
            {
                Success = true,
                StatusCode = 200,
                Message = "Transfer successful",
                Data = new
                {
                    transaction = transactionRecord,
                    senderBalance = appEnvironment == 1 ? wallet.LiveBalance : wallet.TestBalance,
                }
            };
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return new ApiResponse<dynamic> { Success = false, StatusCode = 500, Message = e.Message };
        }
    }
}


        public async Task<ApiResponse<dynamic>> SwitchEnvironmentAsync(SwitchEnvironmentDto switchEnvironmentDto)
        {
            try
            {
                var principal = _httpContextAccessor.HttpContext?.User;
                if (principal == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                }

                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "User not found" };
                }
                //check if user is already in the environment he wants to switch to
                if (user.AppEnvironmentId == switchEnvironmentDto.EnvironmentId)
                {
                    return new ApiResponse<dynamic>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = $"User is already in {(switchEnvironmentDto.EnvironmentId == 1 ? "Live" : "Test")} environment",
                        Data = new
                        {
                            AppEnvironmentId = user.AppEnvironmentId
                        }
                    };
                }
                // change the user environment to live or test
                user.AppEnvironmentId = switchEnvironmentDto.EnvironmentId;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                // switch the user wallet to live or test
                var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId);
                if (wallet == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "Wallet not found" };
                }


                //switch the user wallet to live or test
                wallet.AppEnvironmentId = switchEnvironmentDto.EnvironmentId;
                _dbContext.Wallets.Update(wallet);
                await _dbContext.SaveChangesAsync();

                return new ApiResponse<dynamic>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = $"User environment changed to {(switchEnvironmentDto.EnvironmentId == 1 ? "Live" : "Test")}",
                    Data = new
                    {
                        AppEnvironmentId = user.AppEnvironmentId
                    }
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<dynamic>> ShowBalanceAsync()
        {
            //show balance of user based on the environment he is in
            try
            {
                var principal = _httpContextAccessor.HttpContext?.User;
                if (principal == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 401, Message = "Unauthorized" };
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "User not found" };
                }

                var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId && w.AppEnvironmentId == user.AppEnvironmentId);
                _logger.LogInformation("wallet:********* " + wallet);
                if (wallet == null)
                {
                    return new ApiResponse<dynamic> { Success = false, StatusCode = 404, Message = "Wallet not found" };
                }

                return new ApiResponse<dynamic>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = $"User balance in {(user.AppEnvironmentId == 1 ? "Live" : "Test")} environment",
                    Data = new
                    {
                        Balance = user.AppEnvironmentId == 1 ? wallet.LiveBalance : wallet.TestBalance
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<dynamic> { Success = false, StatusCode = 500, Message = ex.Message };
            }

        }
    }
}
