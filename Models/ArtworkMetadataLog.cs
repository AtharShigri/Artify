using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    [Table("ArtworkMetadataLogs")]
    public class ArtworkMetadataLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ArtworkId { get; set; }

        [MaxLength(100)]
        public string ArtistName { get; set; }

        [MaxLength(500)]
        public string CopyrightText { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }
    }
}
