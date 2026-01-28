using Artify.Api.Data;
using Artify.Api.DTOs.Admin;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Artify.Api.Services.Implementations
{
    public class PlagiarismService : IPlagiarismService
    {
        private readonly ApplicationDbContext _context;

        public PlagiarismService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetPlagiarismLogsAsync()
        {
            var logs = await _context.PlagiarismLogs
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return logs.Select(p => new
            {
                p.Id,
                p.ArtworkId,
                p.SuspectedArtworkId,
                p.SimilarityScore,
                p.IsReviewed,
                p.ActionTaken,
                p.CreatedAt
            });
        }

        public async Task<object?> GetPlagiarismLogByIdAsync(Guid logId)
        {
            var log = await _context.PlagiarismLogs.FirstOrDefaultAsync(p => p.Id == logId);
            if (log == null) return null;

            return new
            {
                log.Id,
                log.ArtworkId,
                log.SuspectedArtworkId,
                log.SimilarityScore,
                log.IsReviewed,
                log.ActionTaken,
                log.Notes,
                log.CreatedAt
            };
        }

        public async Task<object> MarkReviewedAsync(Guid logId, PlagiarismReviewDto dto)
        {
            var log = await _context.PlagiarismLogs.FirstOrDefaultAsync(p => p.Id == logId);
            if (log == null) throw new KeyNotFoundException("Plagiarism log not found.");

            log.IsReviewed = dto.IsReviewed;
            log.Notes = dto.Notes;

            _context.PlagiarismLogs.Update(log);
            await _context.SaveChangesAsync();

            return new { log.Id, log.IsReviewed, log.Notes };
        }

        public async Task<object> TakeActionAsync(Guid logId, PlagiarismReviewDto dto)
        {
            var log = await _context.PlagiarismLogs.FirstOrDefaultAsync(p => p.Id == logId);
            if (log == null) throw new KeyNotFoundException("Plagiarism log not found.");

            log.ActionTaken = dto.ActionTaken;
            log.Notes = dto.Notes;

            _context.PlagiarismLogs.Update(log);
            await _context.SaveChangesAsync();

            return new { log.Id, log.ActionTaken, log.Notes };
        }
    }
}
