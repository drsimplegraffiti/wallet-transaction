write a dotnet application that uses the following

1. BaseEntiy
2. Interface
3. dynamic
4. Data annotation such as Base64string, AllowedValues, DeniedValues, Length, TimeStamp , Range, ConcunrrencyCheck etc
5. Separate concerns: Dtos, Irepository, implementation, request and Standardize API Error response
   6.Optimize query with projection
6. Add effect like optionality, latency, laziness, failure, aggregation
7. Use onmodel in the Datacontext
8. Cancellation token, ensure you avoid repitition
9. Custom object mapping
10. use enum and records
11. use argument
12. use dispose, finalize and Task runner
13. use async and await and Task
    Ensure you write DRY code and clean code. Very clean code!

# Path: dotnet.readme.md

##### Install packages

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson


// serilogs
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File


```

##### BaseEntity

```csharp
public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Timestamp]
    public DateTime CreatedAt { get; set; }

    [Timestamp]
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
```

###### User Model

```csharp
public class User : BaseEntity
{
    [Length(minimumLength: 3, maximumLength: 20)]
    public string FirstName { get; set; }
    [Length(minimumLength: 3, maximumLength: 20)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always))]
    public bool IsVerified { get; set; }

    [Phone(ErrorMessage = "Invalid Phone Number")]
    [Required (ErrorMessage = "Phone Number is required")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone Number must be 11 digits")]
    public string PhoneNumber { get; set; }

    [AllowedValues(20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35))]
    public int Age {get; set;}

    [Required]
    [Range(1, 3)]
    public Role Role { get; set; } = Role.User;


}
```

###### Enum Role

```csharp

public enum Role
{
    Admin = 1,
    User = 2,
    SuperAdmin = 3
}
```

###### Token Model

```csharp
public class Token : BaseEntity
{
    [Base64String]
    public string RefreshToken { get; set; }
    public string JwtToken { get; set; }
    public DateTime Expires { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public User User { get; set; }
}
```

###### AppEnvironment Model

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class AppEnvironment: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
```

##### Wallet Model

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class Wallet: BaseEntity
    {
        public string Name { get; set; } = "Bitcoin";

        public string Symbol { get; set; } = "BTC";
        public string Address { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;

        public decimal Balance { get; set; } = 0;

       public int AppEnvironmentId { get; set; } = 1;


        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? User { get; set; }

        public Wallet()
        {
            Address = Guid.NewGuid().ToString() + "${Symbol}" + Guid.NewGuid().ToString();
        }

        public Wallet(string name, string symbol, string privateKey, string publicKey, decimal balance, string userId)
        {
            Name = name;
            Symbol = symbol;
            PrivateKey = privateKey;
            PublicKey = publicKey;
            Balance = balance;
            UserId = userId;
        }
}
}
```

###### DataContext

```csharp
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Token> Tokens { get; set; }

}

```

###### Dto

Request/CreateUserDto

```csharp
public class CreateUserDto
{
    [Required]
    [Length(minimumLength: 3, maximumLength: 20)]
    public string FirstName { get; set; }
    [Required]
    [Length(minimumLength: 3, maximumLength: 20)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone Number must be 11 digits")]
    public string PhoneNumber { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
    public string Password { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
    public string ConfirmPassword { get; set; }
}

//LoginDto
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}


//RefreshTokenDto
public class RefreshTokenDto
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
```

Standard API Response

```csharp
public class ApiResponse<T>
{
    public ApiResponse(int statusCode, T data = default, string message = null)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            201 => "Created",
            202 => "Accepted",
            203 => "Non-Authoritative Information",
            204 => "No Content",
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Resource Not Found",
            409 => "Resource Conflict",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            _ => null
        };
    }


}

```

###### IPasswordService

```csharp
public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
```

###### Hash Password

```csharp
public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
```

###### ITokenService

```csharp
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
}
```

###### TokenService

```csharp
public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly double _tokenExpirationInMinutes;
    private readonly DataContext _dbContext;


    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"];
        _tokenExpirationInMinutes = Convert.ToDouble(configuration["Jwt:TokenExpirationInMinutes"]);
    }

    public string GenerateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
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
            JwtToken = null, // Null because the token is not issued yet
            Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes),
            UserId = user.Id
        };

        // Save the refresh token to the database
        _dbContext.Tokens.Add(token);
        _dbContext.SaveChanges();



        return refreshToken;
    }
}

```

###### IAuthService

```csharp
public interface IAuthService
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Dto.Request;
using OctApp.Dto.Response;
using OctApp.Models;

namespace OctApp.Services.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse<dynamic>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<dynamic>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);

        Task<ApiResponse<dynamic>> CreateUserAsync(CreateUserDto createUserDto);

    }


}
```

###### AuthService

```csharp
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


    public AuthService(DataContext dbContext, IPasswordService passwordService, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

        public async Task<ApiResponse<dynamic>> CreateUserAsync(CreateUserDto createUserDto)
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


            await _dbContext.SaveChangesAsync();

            return new ApiResponse<dynamic>(200, "User created successfully");
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


```

###### AuthController

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctApp.Dto.Request;
using OctApp.Services.Interface;

namespace OctApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var response = await _authService.RefreshTokenAsync(refreshTokenDto);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto createUserDto)
        {
            var response = await _authService.CreateUserAsync(createUserDto);
            return Ok(response);
        }
    }
}
```

###### Program.cs

```csharp

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OctApp.Data;
using OctApp.Services.Impl;
using OctApp.Services.Interface;
using OctApp.Utils.Impl;
using OctApp.Utils.Interface;

var builder = WebApplication.CreateBuilder(args);

// configure logging
builder.Host.UseSerilog((context, config) =>
{
    config
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
});



// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? string.Empty))
        };
    })
    .AddCookie();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

##### Token Validation filter

```csharp
  public class TokenValidationFilterAttribute : Attribute, IActionFilter
  {
       
       private readonly ILogger<TokenValidationFilterAttribute> _logger;
        private readonly IConfiguration _configuration;

        public TokenValidationFilterAttribute(ILogger<TokenValidationFilterAttribute> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new JsonResult(new ApiResponse<dynamic>(401, message: "Unauthorized"));
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                // attach user to context on successful jwt validation
                context.HttpContext.Items["User"] = userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                context.Result = new JsonResult(new ApiResponse<dynamic>(401, message: "Unauthorized"));
            }
        }
    }
  
```


##### CreateWalletDto
    
```csharp   
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Dto.Request
{
    public class CreateWalletDto
    {
        public string Name { get; set; } = "Binance Naira Balance";

        public string Symbol { get; set; } = "BNB";
        public string Address { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;

        public decimal Balance { get; set; } = 0;

        public int AppEnvironmentId { get; set; } = 1;
        public string UserId { get; set; } = string.Empty;
    }
}
```

##### IWalletService


```csharp
namespace OctApp.Services.Interface
{
    public interface IWalletService
    {
        Task<ApiResponse<dynamic>> CreateWalletAsync(CreateWalletDto createWalletDto);
    }
}
```


##### WalletService

```csharp
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
    public class WalletService : IWalletService
    {
        private readonly DataContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;


        public WalletService(DataContext dbContext, IPasswordService passwordService, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }


        public async Task<ApiResponse<dynamic>> CreateWalletAsync(CreateWalletDto createWalletDto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == createWalletDto.UserId);
            if (user == null)
            {
                return new ApiResponse<dynamic>(404, message: "User not found");
            }

            var wallet = new Wallet
            {
                Name = createWalletDto.Name,
                Symbol = createWalletDto.Symbol,
                Address = Guid.NewGuid().ToString() + "${Symbol}" + Guid.NewGuid().ToString(),
                PrivateKey = Guid.NewGuid().ToString() + "${Symbol}" + Guid.NewGuid().ToString(),
                PublicKey = Guid.NewGuid().ToString() + "${Symbol}" + Guid.NewGuid().ToString(),
                Balance = createWalletDto.Balance,
                UserId = createWalletDto.UserId
            };

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();

        
            return new ApiResponse<dynamic>(200, "Wallet created successfully");
        }
    }
}
```