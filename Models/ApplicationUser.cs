using Microsoft.AspNetCore.Identity;

namespace Artify.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional fields
        public string FullName { get; set; }
        public string RoleType { get; set; } // e.g., "Artist" or "Buyer"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ProfileImageUrl { get; set; }
    }
}
