using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctApp.Data;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Models;
using OctApp.Services.Interface;
using OctApp.Utils;
using OctApp.Utils.Interface;
using RestSharp;

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
                        return ApiResponse<dynamic>.UnauthorizedResponse();
                    }
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                    _logger.LogInformation("userIdClaim:********* " + userIdClaim);

                    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                    {
                        return ApiResponse<dynamic>.UnauthorizedResponse();
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

                    return ApiResponse<dynamic>.OkResponse(new
                    {
                        walletId = wallet.Id,
                    }, "Wallet created successfully");


                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResponse<dynamic>.InternalServerErrorResponse(e.Message);
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
                        return ApiResponse<dynamic>.UnauthorizedResponse();
                    }

                    var userIdClaim = principal.FindFirst("userId")?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    {
                        return ApiResponse<dynamic>.UnauthorizedResponse();
                    }

                    var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                    {
                        return ApiResponse<dynamic>.NotFoundResponse("User not found");
                    }

                    // Check the user's environment
                    var appEnvironment = user.AppEnvironmentId;
                    var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId && w.AppEnvironmentId == appEnvironment);

                    if (wallet == null)
                    {
                        return ApiResponse<dynamic>.NotFoundResponse("Wallet not found");
                    }

                    // ensure you cant transfer money to 
                    if (transferDto.RecipientAccountNumber == wallet.AccountNumber)
                    {
                        return ApiResponse<dynamic>.BadRequestResponse("You can't transfer money to yourself");
                    }

                    // Check if the sender has enough balance
                    decimal senderBalance = appEnvironment == 1 ? wallet.LiveBalance : wallet.TestBalance;

                    if (senderBalance < transferDto.Amount)
                    {
                        return ApiResponse<dynamic>.BadRequestResponse("Insufficient balance");
                    }

                    // Check if the recipient exists
                    var recipient = _dbContext.Wallets.FirstOrDefault(u => u.AccountNumber == transferDto.RecipientAccountNumber);
                    if (recipient == null)
                    {
                        return ApiResponse<dynamic>.NotFoundResponse("Recipient not found");
                    }

                    // Check if the recipient has a wallet
                    var recipientWallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == recipient.Id && w.AppEnvironmentId == appEnvironment);

                    if (recipientWallet == null)
                    {
                        return ApiResponse<dynamic>.NotFoundResponse("Recipient wallet not found");
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

                    return ApiResponse<dynamic>.OkResponse(new
                    {
                        transaction = transactionRecord,
                        senderBalance = appEnvironment == 1 ? wallet.LiveBalance : wallet.TestBalance,
                    }, "Transfer successful");

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResponse<dynamic>.InternalServerErrorResponse(e.Message);
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
                    return ApiResponse<dynamic>.UnauthorizedResponse();
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return ApiResponse<dynamic>.UnauthorizedResponse();
                }

                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("User not found");
                }
                //check if user is already in the environment he wants to switch to
                if (user.AppEnvironmentId == switchEnvironmentDto.EnvironmentId)
                {
                    return ApiResponse<dynamic>.OkResponse(new
                    {
                        AppEnvironmentId = user.AppEnvironmentId
                    }, $"User is already in {(switchEnvironmentDto.EnvironmentId == 1 ? "Live" : "Test")} environment");
                }
                // change the user environment to live or test
                user.AppEnvironmentId = switchEnvironmentDto.EnvironmentId;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                // switch the user wallet to live or test
                var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId);
                if (wallet == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("Wallet not found");
                }


                //switch the user wallet to live or test
                wallet.AppEnvironmentId = switchEnvironmentDto.EnvironmentId;
                _dbContext.Wallets.Update(wallet);
                await _dbContext.SaveChangesAsync();

                return ApiResponse<dynamic>.OkResponse(new
                {
                    AppEnvironmentId = user.AppEnvironmentId
                }, $"User environment changed to {(switchEnvironmentDto.EnvironmentId == 1 ? "Live" : "Test")}");

            }
            catch (Exception ex)
            {
                return ApiResponse<dynamic>.InternalServerErrorResponse(ex.Message);
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
                    return ApiResponse<dynamic>.UnauthorizedResponse();
                }

                var userIdClaim = principal.FindFirst("userId")?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                {
                    return ApiResponse<dynamic>.UnauthorizedResponse();
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("User not found");
                }

                var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId && w.AppEnvironmentId == user.AppEnvironmentId);
                _logger.LogInformation("wallet:********* " + wallet);
                if (wallet == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("Wallet not found");
                }
                return ApiResponse<dynamic>.OkResponse(new
                {
                    Balance = user.AppEnvironmentId == 1 ? wallet.LiveBalance : wallet.TestBalance
                }, $"User balance in {(user.AppEnvironmentId == 1 ? "Live" : "Test")} environment");
            }
            catch (Exception ex)
            {
                return ApiResponse<dynamic>.InternalServerErrorResponse(ex.Message);
            }

        }

        public async Task<string> GetBanksAsync()
        {
            try
            {
                var client = new RestClient("http://localhost:5037/");
                var request = new RestRequest("api/Bank", Method.Get);
                request.AddHeader("Content-Type", "application/json");
                var response = await client.ExecuteAsync(request);
                var dataInfo = new {
                    Banks = response.Content,
                    StatusCode = response.StatusCode,
                    StatusDescription = response.StatusDescription,
                    ErrorMessage = response.ErrorMessage,
                };
                
                return response.Content ?? "No content";
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
