using artifi.Api.Data;
using artifi.Api.DTOs.Admin;
using artifi.Api.Models;
using artifi.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artifi.Api.Services.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _db;

        public AdminDashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<AdminDashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalUsers = await _db.Users.CountAsync();
            var totalArtists = await _db.ArtistProfiles.CountAsync();
            var totalBuyers = totalUsers - totalArtists;
            var totalArtworks = await _db.Artworks.CountAsync(a => !a.IsDeleted);
            var pendingArtworks = await _db.Artworks.CountAsync(a => a.Status == "Pending" && !a.IsDeleted);
            var totalRevenue = await _db.TransactionLogs.SumAsync(t => (decimal?)t.TransactionAmount) ?? 0m;
            var plagiarismAlerts = await _db.PlagiarismLogs.CountAsync(p => !p.IsReviewed);
            
            var activeReports = plagiarismAlerts;

            return new AdminDashboardStatsDto
            {
                TotalUsers = totalUsers,
                TotalArtists = totalArtists,
                TotalBuyers = totalBuyers,
                TotalArtworks = totalArtworks,
                PendingArtworks = pendingArtworks,
                TotalRevenue = totalRevenue,
                ActiveReports = activeReports,
                PlagiarismAlerts = plagiarismAlerts
            };
        }

        public async Task<IEnumerable<RecentUserDto>> GetRecentUsersAsync(int count = 5)
        {
            return await _db.Users
                .Include(u => u.ArtistProfile)
                .OrderByDescending(u => u.CreatedAt)
                .Take(count)
                .Select(u => new RecentUserDto
                {
                    Id = u.Id,
                    Name = u.FullName,
                    Email = u.Email,
                    Role = u.ArtistProfile != null ? "Artist" : "Buyer",
                    Status = u.IsActive ? "Active" : "Inactive",
                    JoinedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PlagiarismReportDto>> GetRecentPlagiarismReportsAsync(int count = 5)
        {
            return await _db.PlagiarismLogs
                .Include(p => p.SuspectedArtwork)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .Select(p => new PlagiarismReportDto
                {
                    Id = p.Id,
                    Type = "Plagiarism",
                    Subject = p.SuspectedArtwork != null ? $"Artwork: {p.SuspectedArtwork.Title}" : "Artwork Fingerprint Match",
                    Status = p.IsReviewed ? "Resolved" : "Pending",
                    CreatedAt = p.CreatedAt,
                    SimilarityScore = p.SimilarityScore
                })
                .ToListAsync();
        }
    }
}
