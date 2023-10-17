using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Models;

namespace OctApp.Utils.Interface
{
    public interface ITokenService
    {
     string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    }
}