// ========================= IAdminUserRepository.cs =========================
using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}
