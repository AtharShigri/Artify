using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Artify.Api.Validation;

namespace Artify.Api.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
        
        [ArtCategory]
        public string Category { get; set; }   
    }
}
