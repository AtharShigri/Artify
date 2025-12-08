// ========================= ArtworkTag.cs =========================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class ArtworkTag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Artwork>? Artworks { get; set; }
    }
}
