using artifi.Api.DTOs.Admin;

namespace artifi.Api.Services.Interfaces
{
    public interface IAdminReportService
    {
        Task<object> GetSummaryReportAsync();

        Task<object> GetUserReportAsync();
        Task<object> GetArtworkReportAsync();
        Task<object> GetSalesReportAsync();

        Task<object> GetUserReportAsync(ReportFilterDto filter);
        Task<object> GetArtworkReportAsync(ReportFilterDto filter);
        Task<object> GetSalesReportAsync(ReportFilterDto filter);

        Task<object> GetMonthlyRegistrationsAsync();
        Task<object> GetTopArtistsAsync();
        Task<object> GetTopArtworksAsync();
        Task<object> GetPlagiarismStatsAsync();
    }
}
