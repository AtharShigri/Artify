using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    [Table("PlagiarismLogs")]
    public class PlagiarismLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArtworkId { get; set; }

        [ForeignKey(nameof(ArtworkId))]
        public Artwork Artwork { get; set; } = null!;

        [Required]
        public Guid SuspectedArtworkId { get; set; }

        [ForeignKey(nameof(SuspectedArtworkId))]
        public Artwork SuspectedArtwork { get; set; } = null!;

        [Required]
        [Range(0, 100)]
        public double SimilarityScore { get; set; }

        public bool IsReviewed { get; set; } = false;

        public bool ActionTaken { get; set; } = false;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
