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