using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Data;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Models;
using OctApp.Services.Interface;
using OctApp.Utils;
using OctApp.Utils.Interface;

namespace OctApp.Services.Impl
{
    public class AuthService : IAuthService
    {

        private readonly DataContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        private readonly IWalletService _walletService;

        private readonly IKeyService _keyService;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(
            DataContext dbContext,
            IPasswordService passwordService,
            ITokenService tokenService,
            IWalletService walletService, IKeyService keyService, IHttpContextAccessor httpContextAccessor = null!)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _walletService = walletService;
            _keyService = keyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<dynamic>> CreateUserAsync(CreateUserDto createUserDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Email == createUserDto.Email);
                    if (user != null)
                    {
                        return ApiResponse<dynamic>.ConflictResponse("User already exists");
                    }

                    var newUser = new User
                    {
                        FirstName = createUserDto.FirstName,
                        LastName = createUserDto.LastName,
                        Email = createUserDto.Email,
                        PhoneNumber = createUserDto.PhoneNumber,
                        PasswordHash = _passwordService.HashPassword(createUserDto.Password)
                    };

                    _dbContext.Users.Add(newUser);
                    await _dbContext.SaveChangesAsync();

                    // Generate wallet keys
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
                        AccountName = $"{newUser.FirstName} {newUser.LastName}",
                        AppEnvironmentId = 2, // Assume 2 represents the test environment
                        UserId = newUser.Id, // Associate the wallet with the newly created user
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _dbContext.Wallets.Add(wallet);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return ApiResponse<dynamic>.OkResponse(new
                    {
                        userId = newUser.Id,
                        walletId = wallet.Id,
                    }, "User created successfully");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResponse<dynamic>.InternalServerErrorResponse(e.Message);
                }
            }
        }




        public async Task<ApiResponse<dynamic>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    //set the status code to 404
                    return ApiResponse<dynamic>.NotFoundResponse("User not found");
                }

                if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return ApiResponse<dynamic>.BadRequestResponse("Invalid password");
                }

                // remove existing tokens for user
                var existingTokens = _dbContext.Tokens.Where(t => t.UserId == user.Id);
                _dbContext.Tokens.RemoveRange(existingTokens);

                // generate new tokens
                var token = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                _dbContext.Tokens.Add(new Token
                {
                    RefreshToken = refreshToken,
                    JwtToken = token,
                    UserId = user.Id,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                await _dbContext.SaveChangesAsync();

                return ApiResponse<dynamic>.OkResponse(new
                {
                    token,
                    refreshToken
                }, "Login successful");
            }
            catch (Exception ex)
            {

                return ApiResponse<dynamic>.InternalServerErrorResponse(ex.Message);
            }

        }

        public async Task<ApiResponse<dynamic>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var token = _dbContext.Tokens.FirstOrDefault(t => t.RefreshToken == refreshTokenDto.RefreshToken);
                if (token == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("Token not found");
                }

                if (token.Expires < DateTime.UtcNow)
                {
                    return ApiResponse<dynamic>.BadRequestResponse("Token has expired");
                }

                var user = _dbContext.Users.FirstOrDefault(u => u.Id == token.UserId);
                if (user == null)
                {
                    return ApiResponse<dynamic>.NotFoundResponse("User not found");
                }

                // remove existing tokens for user
                var existingTokens = _dbContext.Tokens.Where(t => t.UserId == user.Id);
                _dbContext.Tokens.RemoveRange(existingTokens);

                // generate new tokens
                var newToken = _tokenService.GenerateAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);

                _dbContext.Tokens.Add(new Token
                {
                    RefreshToken = newRefreshToken,
                    JwtToken = newToken,
                    UserId = user.Id,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                await _dbContext.SaveChangesAsync();

                return ApiResponse<dynamic>.OkResponse(new
                {
                    // user,
                    token = newToken,
                    refreshToken = newRefreshToken
                }, "Token refreshed successfully");

            }
            catch (Exception e)
            {
                return ApiResponse<dynamic>.InternalServerErrorResponse(e.Message);
            }
        }
    }
}