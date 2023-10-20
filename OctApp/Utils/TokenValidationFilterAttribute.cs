using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using OctApp.Dto.Response;



namespace OctApp.Utils
{
    public class TokenValidationFilterAttribute : Attribute, IActionFilter
    {

        private readonly ILogger<TokenValidationFilterAttribute> _logger;
        private readonly IConfiguration _configuration;

        public TokenValidationFilterAttribute(ILogger<TokenValidationFilterAttribute> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // skip token validation if the method is decorated with [SkipTokenValidation]
            var skipTokenValidation = context.ActionDescriptor?.EndpointMetadata
                .Any(em => em is SkipTokenValidationAttribute) == true;

            if (skipTokenValidation)
            {
                _logger.LogInformation("OnActionExecuting==================== Skip token validation");
                return; // Skip token validation if the custom attribute is present
            }


            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.HttpContext.Response.StatusCode = 401;
                 context.Result = new ObjectResult(new ApiResponse<dynamic>{
                    Success = false,
                    StatusCode = 401,
                    Message = "Unauthorized, provide a token"
                 });
                    return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JWT:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                 
                     ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true 
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }));
            } catch(ArgumentException ex)
            {
                _logger.LogError(ex, "Error validating token");
                context.Result = new ObjectResult(new ApiResponse<dynamic>{
                    Success = false,
                    StatusCode = 401,
                    Message = "Error validating token"
                 });
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Error validating token");
                context.Result = new ObjectResult(new ApiResponse<dynamic>{
                    Success = false,
                    StatusCode = 401,
                    Message = "Unauthorized"
                 });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                context.Result = new ObjectResult(new ApiResponse<dynamic>{
                    Success = false,
                    StatusCode = 401,
                    Message = "Unauthorized"
                 });
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class SkipTokenValidationAttribute : Attribute
{
}