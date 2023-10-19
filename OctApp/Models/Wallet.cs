using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class Wallet : BaseEntity
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string TestPrivateKey { get; set; } = string.Empty;
        public string TestPublicKey { get; set; } = string.Empty;
        public string LivePrivateKey { get; set; } = string.Empty;
        public string LivePublicKey { get; set; } = string.Empty;

        public decimal TestBalance { get; set; } = 0;
        public decimal LiveBalance { get; set; } = 0;

        public int AppEnvironmentId { get; set; } = 2;

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore()]
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? User { get; set; }

        public decimal GetActiveBalance()
        {
            if (AppEnvironmentId == 1)
            {
                return LiveBalance;
            }
            else if (AppEnvironmentId == 2)
            {
                return TestBalance;
            }
            else
            {
                return 0;
            }
        }

        public string GetActivePrivateKey()
        {
            if (AppEnvironmentId == 1)
            {
                return LivePrivateKey;
            }
            else if (AppEnvironmentId == 2)
            {
                return TestPrivateKey;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetActivePublicKey()
        {
            if (AppEnvironmentId == 1)
            {
                return LivePublicKey;
            }
            else if (AppEnvironmentId == 2)
            {
                return TestPublicKey;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
