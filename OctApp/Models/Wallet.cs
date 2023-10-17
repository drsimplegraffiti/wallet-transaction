using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class Wallet : BaseEntity
    {
        public string Name { get; set; } = "Binance Naira Balance";

        public string Symbol { get; set; } = "BNB";
        public string Address { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;

        public decimal Balance { get; set; } = 0;

        public int AppEnvironmentId { get; set; } = 1;


        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? User { get; set; }
    }
}