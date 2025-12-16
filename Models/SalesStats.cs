// ========================= SalesStats.cs =========================
namespace Artify.Api.Models
{
    public class SalesStats
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedTransactions { get; set; }
        public int FailedTransactions { get; set; }
    }
}
