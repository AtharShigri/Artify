// ========================= AdminUserService.cs =========================
using artifi.Api.DTOs.Admin;
using artifi.Api.Mappings;
using artifi.Api.Models;
using artifi.Api.Repositories.Interfaces;
using artifi.Api.Services.Interfaces;

namespace artifi.Api.Services.Implementations
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

        public async Task<object?> GetUserByIdAsync(Guid userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            return user == null ? null : AdminUserMappings.ToAdminUserDto(user);
        }

        public async Task<object> UpdateUserStatusAsync(Guid userId, UpdateUserStatusDto dto)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user = AdminUserMappings.ApplyStatusUpdate(user, dto);
            var updated = await _repository.UpdateUserAsync(user);
            return AdminUserMappings.ToAdminUserDto(updated);
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            return await _repository.SoftDeleteUserAsync(userId);
        }
    }
}
