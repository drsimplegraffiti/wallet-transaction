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
    }
}