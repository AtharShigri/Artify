// ========================= IAdminUserRepository.cs =========================
using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}
