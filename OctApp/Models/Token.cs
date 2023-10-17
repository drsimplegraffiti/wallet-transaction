using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OctApp.Models
{
    public class Token
    {
    public int Id {get; set;}
    public string RefreshToken { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public DateTime Expires { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public User? User { get; set; } 
    }
}