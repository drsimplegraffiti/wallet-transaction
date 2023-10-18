using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OctApp.Data;
using OctApp.Models;
using OctApp.Utils.Interface;

namespace OctApp.Utils.Impl
{
    public class TokenService: ITokenService
{
    private readonly string _secretKey;
    private readonly double _tokenExpirationInMinutes;
    private readonly DataContext _dbContext;


        public TokenService(IConfiguration configuration, DataContext dbContext)
        {
            _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("JWT:SecretKey");
            _tokenExpirationInMinutes = Convert.ToDouble(configuration["Jwt:TokenExpirationInMinutes"]);
            _dbContext = dbContext;
        }

        public string GenerateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var token = new Token
        {
            RefreshToken = refreshToken,
            JwtToken = GenerateAccessToken(user),
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes),
            UserId = user.Id
        };
        return refreshToken;
    }
}
}