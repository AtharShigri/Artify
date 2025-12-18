using System.ComponentModel.DataAnnotations;

namespace Artify.Api.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Artwork>? Artworks { get; set; }
        public ICollection<Service>? Services { get; set; }

    }

   

}
