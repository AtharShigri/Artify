using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class AIHashRecord
    {
        [Key]
        public int HashId { get; set; }

        public Guid ArtworkId { get; set; }
        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }

        public string HashValue { get; set; }
        public float PlagiarismScore { get; set; }
        public bool IsFlagged { get; set; }
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    }
}
