using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artify.Api.Repositories.Implementations
{
    public class AdminArtworkRepository : IAdminArtworkRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminArtworkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artwork>> GetAllArtworksAsync()
        {
            return await _context.Artworks
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Artwork>> GetPendingArtworksAsync()
        {
            return await _context.Artworks
                .Where(a => a.Status == "Pending") // consider using a constant or enum
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Artwork?> GetArtworkByIdAsync(int artworkId)
        {
            return await _context.Artworks
                .FirstOrDefaultAsync(a => a.ArtworkId == artworkId);
        }

        public async Task<Artwork> UpdateArtworkAsync(Artwork artwork)
        {
            if (artwork == null) throw new ArgumentNullException(nameof(artwork));

            _context.Artworks.Update(artwork);
            await _context.SaveChangesAsync();
            return artwork;
        }

        public async Task<bool> RemoveArtworkAsync(int artworkId)
        {
            var artwork = await _context.Artworks
                .FirstOrDefaultAsync(a => a.ArtworkId == artworkId);

            if (artwork == null) return false;

            artwork.IsDeleted = true; // make sure IsDeleted exists in Artwork

            _context.Artworks.Update(artwork);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
