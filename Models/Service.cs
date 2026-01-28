// ========================= Service.cs =========================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Duration { get; set; } // Compatibility
        public string? Category { get; set; } // Compatibility

        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? CategoryEntity { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public Guid ArtistId { get; set; }

        [ForeignKey("ArtistId")]
        public ArtistProfile? ArtistProfile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
