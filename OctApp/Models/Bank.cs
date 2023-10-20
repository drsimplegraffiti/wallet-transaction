using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class Bank: BaseEntity
    {
        public string BankName { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
    }
}