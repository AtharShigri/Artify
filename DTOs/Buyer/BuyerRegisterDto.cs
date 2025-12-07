using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Buyer
{
    public class BuyerRegisterDto
    {
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required][StringLength(100, MinimumLength = 2)] public string FullName { get; set; } = string.Empty;
        [Required][StringLength(100, MinimumLength = 6)] public string Password { get; set; } = string.Empty;
        [Phone] public string? PhoneNumber { get; set; }
    }

    public class BuyerUpdateDto
    {
        [StringLength(100, MinimumLength = 2)] public string? FullName { get; set; }
        [Phone] public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

    public class BuyerProfileResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TotalOrders { get; set; }
        public int TotalReviews { get; set; }
    }
}