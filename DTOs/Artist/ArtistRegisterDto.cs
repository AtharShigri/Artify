using System.Security.Claims;

namespace Artify.Api.DTOs.Artist
{
    public class ArtistRegisterDto
    {
        public string FullName { get; set; }
        public String Email { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }     // Painter, Singer, Calligrapher, etc.
    }
}
