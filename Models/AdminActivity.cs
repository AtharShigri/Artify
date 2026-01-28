using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class AdminActivity
    {
        [Key]
        public int ActivityId { get; set; }

        public Guid AdminId { get; set; }
        [ForeignKey("AdminId")]
        public ApplicationUser Admin { get; set; }

        public string TargetType { get; set; } // e.g. "Artwork", "User", "Order"
        public string TargetId { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
