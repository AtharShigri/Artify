using System.ComponentModel.DataAnnotations;

namespace artifi.Api.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
