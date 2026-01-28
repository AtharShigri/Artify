using Artify.Api.DTOs.Admin;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class AdminReportService : IAdminReportService
    {
        private readonly IAdminReportRepository _repository;

        public AdminReportService(IAdminReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> GetSummaryReportAsync()
            => await _repository.GetSummaryReportAsync();

        public async Task<object> GetUserReportAsync()
            => await _repository.GetUserReportAsync(new ReportFilterDto());

        public async Task<object> GetArtworkReportAsync()
            => await _repository.GetArtworksReportAsync(new ReportFilterDto());

        public async Task<object> GetSalesReportAsync()
            => await _repository.GetSalesReportAsync(new ReportFilterDto());

        public async Task<object> GetUserReportAsync(ReportFilterDto filter)
            => await _repository.GetUserReportAsync(filter);

        public async Task<object> GetArtworkReportAsync(ReportFilterDto filter)
            => await _repository.GetArtworksReportAsync(filter);

        public async Task<object> GetSalesReportAsync(ReportFilterDto filter)
            => await _repository.GetSalesReportAsync(filter);

        public async Task<object> GetMonthlyRegistrationsAsync()
            => await _repository.GetMonthlyRegistrationsAsync();

        public async Task<object> GetTopArtistsAsync()
            => await _repository.GetTopArtistsAsync();

        public async Task<object> GetTopArtworksAsync()
            => await _repository.GetTopArtworksAsync();

        public async Task<object> GetPlagiarismStatsAsync()
            => await _repository.GetPlagiarismStatsAsync();
    }
}
