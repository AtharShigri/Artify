using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Artist
{
    public class UpdateProfileImageDto
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
