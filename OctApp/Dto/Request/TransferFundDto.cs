using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Dto.Request
{
    public class TransferFundDto
    {
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; }  = string.Empty;
        public decimal Amount { get; set; } = 0;
        public string Symbol { get; set; } = string.Empty;
    }
}