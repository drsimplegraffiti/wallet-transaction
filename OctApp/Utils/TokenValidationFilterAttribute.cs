using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new JsonResult(new ApiResponse<dynamic>(401, message: "Unauthorized"));
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JWT:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    // ValidateIssuer = true,
                    // ValidateAudience = true,
                    // ValidateLifetime = true,
                    // ValidateIssuerSigningKey = true,
                    // ValidIssuer = _configuration["JWT:Issuer"],
                    // ValidAudience = _configuration["JWT:Audience"],
                    // IssuerSigningKey = new SymmetricSecurityKey(key)
                     ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Set to true if you want to validate the issuer
                    ValidateAudience = false, // Set to true if you want to validate the audience
                    ValidateLifetime = true // Set to true if you want to validate token expiration
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                context.HttpContext.Items["User"] = userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                context.Result = new JsonResult(new ApiResponse<dynamic>(401, message: "Unauthorized"));
            }
        }
    }
}