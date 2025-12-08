// ========================= AdminUserService.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Mappings;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _repository;

        public AdminUserService(IAdminUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            return users.Select(u => AdminUserMappings.ToAdminUserDto(u));
        }

        public async Task<object?> GetUserByIdAsync(string userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            return user == null ? null : AdminUserMappings.ToAdminUserDto(user);
        }

        public async Task<object> UpdateUserStatusAsync(string userId, UpdateUserStatusDto dto)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user = AdminUserMappings.ApplyStatusUpdate(user, dto);
            var updated = await _repository.UpdateUserAsync(user);
            return AdminUserMappings.ToAdminUserDto(updated);
        }

        public async Task<bool> SoftDeleteUserAsync(string userId)
        {
            return await _repository.SoftDeleteUserAsync(userId);
        }
    }
}
