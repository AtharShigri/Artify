using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtServiceRepository : IArtServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Service service)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Service>> GetAllByArtistAsync(Guid artistId)
        {
            return await _context.Services
                .Where(s => s.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<Service> GetByIdAsync(Guid serviceId)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);
        }

        public async Task UpdateAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ServiceExistsAsync(Guid serviceId, Guid artistId)
        {
            return await _context.Services
                .AnyAsync(s => s.Id == serviceId && s.ArtistId == artistId);
        }
    }
}
