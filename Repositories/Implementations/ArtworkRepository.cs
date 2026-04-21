using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtworkRepository : IArtworkRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtworkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Artwork artwork)
        {
            await _context.Artworks.AddAsync(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Artwork artwork)
        {
            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Artwork>> GetAllByArtistAsync(Guid artistId)
        {
            IQueryable<Artwork> query = _context.Artworks
                .Include(a => a.Tags)
                .Include(a => a.CategoryEntity)
                .Include(a => a.ArtistProfile)
                    .ThenInclude(ap => ap.User);

            if (artistId != Guid.Empty)
            {
                query = query.Where(a => a.ArtistProfileId == artistId);
            }

            return await query.ToListAsync();
        }

        public async Task<Artwork> GetByIdAsync(Guid artworkId)
        {
            return await _context.Artworks
                .Include(a => a.Tags)
                .Include(a => a.CategoryEntity)
                .Include(a => a.ArtistProfile)
                    .ThenInclude(ap => ap.User)
                .FirstOrDefaultAsync(a => a.ArtworkId == artworkId);
        }

        public async Task UpdateAsync(Artwork artwork)
        {
            _context.Artworks.Update(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ArtworkExistsAsync(Guid artworkId, Guid artistId)
        {
            return await _context.Artworks
                .AnyAsync(a => a.ArtworkId == artworkId && a.ArtistProfileId == artistId);
        }

        public async Task<IEnumerable<Artwork>> GetArtworksByArtistIdsAsync(IEnumerable<Guid> artistIds)
        {
            return await _context.Artworks
                .Where(a => artistIds.Contains(a.ArtistProfileId))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
