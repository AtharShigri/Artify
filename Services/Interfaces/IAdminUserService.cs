// ========================= IAdminUserService.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Models;

namespace Artify.Api.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<IEnumerable<object>> GetAllUsersAsync();
        Task<object?> GetUserByIdAsync(string userId);
        Task<object> UpdateUserStatusAsync(string userId, UpdateUserStatusDto dto);
        Task<bool> SoftDeleteUserAsync(string userId);
    }
}
