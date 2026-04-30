// ========================= AdminUserRepository.cs =========================
using artifi.Api.Data;
using artifi.Api.Models;
using artifi.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace artifi.Api.Repositories.Implementations
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.ArtistProfile)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.ArtistProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
