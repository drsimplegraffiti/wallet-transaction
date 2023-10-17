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
    public class AuthService : IAuthService
    {

        private readonly DataContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        private readonly IWalletService _walletService;


        public AuthService(DataContext dbContext, IPasswordService passwordService, ITokenService tokenService, IWalletService walletService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _walletService = walletService;
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
                        return new ApiResponse<dynamic>(400, message: "Email already exists");
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

                    // Create a wallet for the user
                    var (walletAddress, publicKey, privateKey) = _walletService.GenerateWallet();
                    var wallet = new Wallet
                    {
                        Address = walletAddress,
                        PublicKey = publicKey,
                        PrivateKey = privateKey,
                        Balance = 0,
                        Name = "Binance Naira Balance",
                        Symbol = "BNB",
                        AppEnvironmentId = 1,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    // Associate the wallet with the newly created user
                    wallet.User = newUser;

                    _dbContext.Wallets.Add(wallet);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                    return new ApiResponse<dynamic>(200, "User created successfully");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    return new ApiResponse<dynamic>(400, message: "Error creating user + " + e.Message);
                }
            }
        }


        public async Task<ApiResponse<dynamic>> LoginAsync(LoginDto loginDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return new ApiResponse<dynamic>(404, message: "User not found");
            }

            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return new ApiResponse<dynamic>(400, message: "Invalid password");
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

            return new ApiResponse<dynamic>(200, new
            {
                // user = user,
                token,
                refreshToken
            });

        }

        public async Task<ApiResponse<dynamic>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var token = _dbContext.Tokens.FirstOrDefault(t => t.RefreshToken == refreshTokenDto.RefreshToken);
            if (token == null)
            {
                return new ApiResponse<dynamic>(404, message: "Token not found");
            }

            if (token.Expires < DateTime.UtcNow)
            {
                return new ApiResponse<dynamic>(400, message: "Token has expired");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == token.UserId);
            if (user == null)
            {
                return new ApiResponse<dynamic>(404, message: "User not found");
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

            return new ApiResponse<dynamic>(200, new
            {
                // user = user,
                token = newToken,
                refreshToken = newRefreshToken
            });
        }
    }
}