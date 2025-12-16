// ========================= AdminReportMappings.cs =========================
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public static class AdminReportMappings
    {
        public static object ToSummaryReport(SystemSummary summary)
        {
            return new
            {
                summary.TotalUsers,
                summary.TotalArtists,
                summary.TotalArtworks,
                summary.TotalSales,
                summary.MonthlyRegistrations
            };
        }

        public static object ToUserReport(UserStats stats)
        {
            return new
            {
                stats.ActiveUsers,
                stats.BlockedUsers,
                stats.NewUsersThisMonth
            };
        }

        public static object ToArtworkReport(ArtworkStats stats)
        {
            return new
            {
                stats.TotalArtworks,
                stats.PendingApproval,
                stats.ApprovedArtworks,
                stats.RejectedArtworks
            };
        }

        public static object ToSalesReport(SalesStats stats)
        {
            return new
            {
                stats.TotalRevenue,
                stats.TotalOrders,
                stats.CompletedTransactions,
                stats.FailedTransactions
            };
        }
    }
}
