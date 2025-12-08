// ========================= PlagiarismReviewDto.cs =========================
namespace Artify.Api.DTOs.Admin
{
    public class PlagiarismReviewDto
    {
        public bool IsReviewed { get; set; }
        public bool ActionTaken { get; set; }
        public string? Notes { get; set; }
    }
}
