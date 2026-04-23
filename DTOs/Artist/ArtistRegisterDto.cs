using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Artify.Api.Validation;

namespace Artify.Api.DTOs.Artist
{
    public class ArtistRegisterDto
    {
        [Required]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [ArtCategory]
        public string Category { get; set; }     // Painter, Singer, Calligrapher, etc.
    }
}
