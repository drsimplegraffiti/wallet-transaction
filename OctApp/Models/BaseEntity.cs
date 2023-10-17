
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OctApp.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        // [Timestamp]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // [Timestamp]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;
    }
}