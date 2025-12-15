// ========================= SystemSummary.cs =========================
namespace Artify.Api.Models
{
    public class SystemSummary
    {
        public int TotalUsers { get; set; }
        public int TotalArtists { get; set; }
        public int TotalArtworks { get; set; }
        public decimal TotalSales { get; set; }
        public int MonthlyRegistrations { get; set; }
    }
}
