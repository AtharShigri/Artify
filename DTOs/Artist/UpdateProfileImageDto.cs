using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace artifi.Api.DTOs.Artist
{
    public class UpdateProfileImageDto
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
