using Artify.Api.DTOs.Admin;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IAdminReportRepository
    {
        Task<object> GetSummaryReportAsync();

        Task<object> GetUserReportAsync(ReportFilterDto filter);
        Task<object> GetArtworksReportAsync(ReportFilterDto filter);
        Task<object> GetSalesReportAsync(ReportFilterDto filter);

        Task<object> GetMonthlyRegistrationsAsync();
        Task<object> GetTopArtistsAsync();
        Task<object> GetTopArtworksAsync();
        Task<object> GetPlagiarismStatsAsync();
    }
}
