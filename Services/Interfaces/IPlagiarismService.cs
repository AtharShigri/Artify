// ========================= IPlagiarismService.cs =========================
using artifi.Api.DTOs.Admin;

namespace artifi.Api.Services.Interfaces
{
    public interface IPlagiarismService
    {
        Task<IEnumerable<object>> GetPlagiarismLogsAsync();
        Task<object?> GetPlagiarismLogByIdAsync(Guid logId);
        Task<object> MarkReviewedAsync(Guid logId, PlagiarismReviewDto dto);
        Task<object> TakeActionAsync(Guid logId, PlagiarismReviewDto dto);
    }
}
