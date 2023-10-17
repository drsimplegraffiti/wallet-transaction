using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Dto.Request
{
    public class CreateUserDto
    {
        
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 20 characters")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 20 characters")]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone Number must be 11 digits")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
    public string Password { get; set; } = string.Empty;
    }
}