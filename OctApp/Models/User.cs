using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class User: BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool IsVerified { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone Number must be 11 digits")]
        public string PhoneNumber { get; set; } = string.Empty;

        public int Age { get; set; }

        public int AppEnvironmentId { get; set; } = 2;

        [Required]
        [Range(1, 3)]
        public Role Role { get; set; } = Role.User;

        //wallets
        public ICollection<Wallet>? Wallets { get; set; } = default!;

        
    }

    public enum Role
    {
        Admin = 1,
        User = 2,
        SuperAdmin = 3
    }

}

