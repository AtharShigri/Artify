using Microsoft.AspNetCore.Identity;

namespace Artify.Api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RoleType { get; set; } // Admin, Artist, or Buyer

    // Navigation Properties
    public virtual ArtistProfile? ArtistProfile { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
}
