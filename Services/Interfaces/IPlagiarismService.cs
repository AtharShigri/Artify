// ========================= IPlagiarismService.cs =========================
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Services.Interfaces
{
    public interface IPlagiarismService
    {
        Task<IEnumerable<object>> GetPlagiarismLogsAsync();
        Task<object?> GetPlagiarismLogByIdAsync(Guid logId);
        Task<object> MarkReviewedAsync(Guid logId, PlagiarismReviewDto dto);
        Task<object> TakeActionAsync(Guid logId, PlagiarismReviewDto dto);
    }
}
