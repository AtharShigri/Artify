using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ProtectionRepository : IProtectionRepository
    {
        private readonly ApplicationDbContext _context;

        public ProtectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Hash ──────────────────────────────────────────────────────────────

        public async Task AddHashRecordAsync(ArtworkHash hash)
        {
            await _context.ArtworkHashes.AddAsync(hash);
            await _context.SaveChangesAsync();
        }

        public async Task<ArtworkHash?> GetArtworkHashAsync(Guid artworkId)
        {
            return await _context.ArtworkHashes
                .FirstOrDefaultAsync(h => h.ArtworkId == artworkId);
        }

        public async Task<IEnumerable<ArtworkHash>> GetAllHashesAsync()
        {
            return await _context.ArtworkHashes
                .Include(h => h.Artwork)
                    .ThenInclude(a => a.ArtistProfile)
                .ToListAsync();
        }

        // ── Metadata ─────────────────────────────────────────────────────────

        public async Task AddMetadataLogAsync(ArtworkMetadataLog log)
        {
            await _context.ArtworkMetadataLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<ArtworkMetadataLog?> GetMetadataAsync(Guid artworkId)
        {
            return await _context.ArtworkMetadataLogs
                .FirstOrDefaultAsync(m => m.ArtworkId == artworkId);
        }

        // ── Plagiarism ────────────────────────────────────────────────────────

        public async Task AddPlagiarismLogAsync(PlagiarismLog log)
        {
            await _context.PlagiarismLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
