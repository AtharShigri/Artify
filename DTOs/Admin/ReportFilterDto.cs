// ========================= ReportFilterDto.cs =========================
namespace Artify.Api.DTOs.Admin
{
    public class ReportFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Category { get; set; }
        public string? SortBy { get; set; }
        public int Page {  get; set; }
        public int PageSize { get; internal set; }
    }
}
