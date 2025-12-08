using System;
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
    }
}
