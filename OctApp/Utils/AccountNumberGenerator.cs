using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Utils
{
    public static class AccountNumberGenerator
    {
        public static string GenerateAccountNumber()
        {
            var random = new Random();
            var accountNumber = string.Empty;
            for (var i = 0; i < 10; i++)
            {
                accountNumber += random.Next(0, 9).ToString();
            }
            return accountNumber;
        }
        
    }
}