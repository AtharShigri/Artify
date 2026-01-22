namespace Artify.Api.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
    }
}
