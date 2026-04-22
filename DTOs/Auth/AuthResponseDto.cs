namespace Artify.Api.DTOs.Auth
{
    public class AuthResponseDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public int UserType { get; set; } // 0 = Individual, 1 = Agency
    }
}
