// ========================= UpdateUserStatusDto.cs =========================
namespace Artify.Api.DTOs.Admin
{
    public class UpdateUserStatusDto
    {
        public bool IsActive { get; set; }
        public string? Reason { get; set; }
    }
}
