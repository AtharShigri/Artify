using System;
﻿// ========================= PlagiarismLog.cs =========================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    [Table("PlagiarismLogs")]
    public class PlagiarismLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [MaxLength(200)]
        public string FileName { get; set; }

        [MaxLength(1000)]
        public string Result { get; set; }

        public DateTime CheckedAt { get; set; }

        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }
    public class PlagiarismLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArtworkId { get; set; }

        [ForeignKey("ArtworkId")]
        public Artwork? Artwork { get; set; }

        [Required]
        public Guid SuspectedArtworkId { get; set; }

        [ForeignKey("SuspectedArtworkId")]
        public Artwork? SuspectedArtwork { get; set; }

        [Required]
        [Range(0, 100)]
        public double SimilarityScore { get; set; }

        public bool IsReviewed { get; set; } = false;

        public bool ActionTaken { get; set; } = false;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
