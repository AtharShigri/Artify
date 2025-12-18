using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    [Table("ArtworkHashes")]
    public class ArtworkHash
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ArtworkId { get; set; }

        [Required]
        [MaxLength(128)]
        public string HashValue { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }
    }
}
