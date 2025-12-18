using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Artist
{
    public class PlagiarismCheckDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
