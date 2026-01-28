using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Artist
{
    public class WatermarkDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
