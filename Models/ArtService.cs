using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    [Table("ArtServices")]
    public class ArtService
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string Duration { get; set; } // e.g., "2 hours", "Per event"

        [MaxLength(50)]
        public string Category { get; set; }

        // Navigation property
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }
    }
}
