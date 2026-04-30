// ========================= IAdminUserService.cs =========================
using artifi.Api.DTOs.Admin;
using artifi.Api.Models;

namespace artifi.Api.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<IEnumerable<object>> GetAllUsersAsync();
        Task<object?> GetUserByIdAsync(Guid userId);
        Task<object> UpdateUserStatusAsync(Guid userId, UpdateUserStatusDto dto);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}
