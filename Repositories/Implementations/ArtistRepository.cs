using System.Security.Claims;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Artist artist)
        {
            await _context.Set<Artist>().AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Artist artist)
        {
            _context.Set<Artist>().Remove(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<Artist> GetByEmailAsync(string email)
        {
            return await _context.Set<Artist>()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Artist> GetByIdAsync(Guid artistId)
        {
            return await _context.Set<Artist>()
                .FirstOrDefaultAsync(u => u.Id == artistId);
        }

        public async Task UpdateAsync(Artist artist)
        {
            _context.Set<Artist>().Update(artist);
            await _context.SaveChangesAsync();
        }

        public Guid GetArtistId(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
            {
                return id;
            }
            return Guid.Empty;
        }
    }
}
