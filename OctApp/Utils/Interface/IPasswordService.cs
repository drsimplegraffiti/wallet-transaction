using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Utils.Interface
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}