// ========================= AdminReportRepository.cs =========================
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.DTOs.Admin;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Repositories.Implementations
{
    public class AdminReportRepository : IAdminReportRepository
    {
        private readonly ApplicationDbContext _db;

        public AdminReportRepository(ApplicationDbContext db) => _db = db;

        // ---------------- Summary Report ----------------
        public async Task<object> GetSummaryReportAsync()
        {
            var usersCount = await _db.Users.CountAsync(u => u.IsActive);
            var activeUsers = await _db.Users.CountAsync(u => u.IsActive);
            var pendingArtworks = await _db.Artworks.CountAsync(a => a.Status == "Pending" && !a.IsDeleted);
            var totalSales = await _db.TransactionLogs.SumAsync(t => (decimal?)t.TransactionAmount) ?? 0m;
            var plagiarismLogs = await _db.PlagiarismLogs.CountAsync();

            return new
            {
                Users = usersCount,
                ActiveUsers = activeUsers,
                PendingArtworks = pendingArtworks,
                TotalSales = totalSales,
                PlagiarismLogs = plagiarismLogs
            };
        }

        // ---------------- Users Report ----------------
        public async Task<object> GetUserReportAsync(ReportFilterDto filter)
        {
            var q = _db.Users.AsNoTracking().Where(u => u.IsActive);

            if (filter.FromDate.HasValue) q = q.Where(u => u.CreatedAt >= filter.FromDate.Value);
            if (filter.ToDate.HasValue) q = q.Where(u => u.CreatedAt <= filter.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SortBy) &&
                filter.SortBy.Equals("monthly", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = await q.GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                                     .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Count = g.Count() })
                                     .OrderBy(x => x.Year).ThenBy(x => x.Month)
                                     .ToListAsync();
                return grouped;
            }

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(u => u.CreatedAt)
                               .Skip((filter.Page - 1) * filter.PageSize)
                               .Take(filter.PageSize)
                               .Select(u => new { u.Id, u.FullName, u.Email, u.CreatedAt, u.IsActive })
                               .ToListAsync();

            return new { Total = total, Items = items, Page = filter.Page, PageSize = filter.PageSize };
        }

        // ---------------- Artworks Report ----------------
        public async Task<object> GetArtworksReportAsync(ReportFilterDto filter)
        {
            var q = _db.Artworks.AsNoTracking().Where(a => !a.IsDeleted);

            if (filter.FromDate.HasValue) q = q.Where(a => a.CreatedAt >= filter.FromDate.Value);
            if (filter.ToDate.HasValue) q = q.Where(a => a.CreatedAt <= filter.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SortBy) &&
                filter.SortBy.Equals("status", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = await q.GroupBy(a => a.Status)
                                     .Select(g => new { Status = g.Key, Count = g.Count() })
                                     .ToListAsync();
                return grouped;
            }

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(a => a.CreatedAt)
                               .Skip((filter.Page - 1) * filter.PageSize)
                               .Take(filter.PageSize)
                               .Select(a => new
                               {
                                   a.ArtworkId,
                                   a.Title,
                                   ArtistId = a.ArtistProfileId,
                                   a.Status,
                                   a.CreatedAt
                               })
                               .ToListAsync();

            return new { Total = total, Items = items, Page = filter.Page, PageSize = filter.PageSize };
        }

        // ---------------- Sales Report ----------------
        public async Task<object> GetSalesReportAsync(ReportFilterDto filter)
        {
            var q = _db.TransactionLogs.AsNoTracking().AsQueryable();

            if (filter.FromDate.HasValue) q = q.Where(t => t.TransactionDate >= filter.FromDate.Value);
            if (filter.ToDate.HasValue) q = q.Where(t => t.TransactionDate <= filter.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SortBy) &&
                filter.SortBy.Equals("daily", StringComparison.OrdinalIgnoreCase))
            {
                var grouped = await q.GroupBy(t => t.TransactionDate.Date)
                                     .Select(g => new { Date = g.Key, TotalAmount = g.Sum(x => x.TransactionAmount), Count = g.Count() })
                                     .OrderBy(x => x.Date)
                                     .ToListAsync();
                return grouped;
            }

            var total = await q.SumAsync(t => (decimal?)t.TransactionAmount) ?? 0m;
            var count = await q.CountAsync();

            var items = await q.OrderByDescending(t => t.TransactionDate)
                               .Take(100)
                               .Select(t => new
                               {
                                   t.TransactionId,
                                   t.Order,
                                   t.TransactionAmount,
                                   t.PaymentMethod,
                                   t.Status,
                                   t.TransactionDate
                               })
                               .ToListAsync();

            return new { TotalAmount = total, Count = count, Items = items };
        }

        public async Task<object> GetMonthlyRegistrationsAsync()
        {
            return await _db.Users
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }

        public async Task<object> GetTopArtistsAsync()
        {
            return await _db.ArtistProfiles
                .OrderByDescending(a => a.Rating)
                .Take(10)
                .Select(a => new
                {
                    a.ArtistProfileId,
                    a.Rating,
                    a.User

                })
                .ToListAsync();
        }

        public async Task<object> GetTopArtworksAsync()
        {
            return await _db.Artworks
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.ViewsCount)
                .Take(10)
                .Select(a => new
                {
                    a.ArtworkId,
                    a.Title,
                    a.ViewsCount,
                    a.Status
                })
                .ToListAsync();
        }

        public async Task<object> GetPlagiarismStatsAsync()
        {
            return await _db.PlagiarismLogs
                .GroupBy(p => p.IsReviewed)
                .Select(g => new
                {
                    IsReviewed = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

    }
}
