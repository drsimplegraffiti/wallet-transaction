using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Dto.Request
{
    public class TransferFundDto
    {
        // public string SenderAccountNumber { get; set; } = string.Empty;
        public string RecipientAccountNumber { get; set; }  = string.Empty;
        public decimal Amount { get; set; } = 0;
    }
}