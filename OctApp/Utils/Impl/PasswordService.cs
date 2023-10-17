using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OctApp.Utils.Interface;

namespace OctApp.Utils.Impl
{
    public class PasswordService: IPasswordService
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
}