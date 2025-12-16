// ========================= ArtworkStats.cs =========================
namespace Artify.Api.Models
{
    public class ArtworkStats
    {
        public int TotalArtworks { get; set; }
        public int PendingApproval { get; set; }
        public int ApprovedArtworks { get; set; }
        public int RejectedArtworks { get; set; }
    }
}
