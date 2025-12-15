// ========================= AdminReportService.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Mappings;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artify.Api.Services.Implementations
{
    public class AdminReportService : IAdminReportService
    {
        private readonly IAdminReportRepository _repository;

        public AdminReportService(IAdminReportRepository repository)
        {
            _repository = repository;
        }

        // Summary report
        public async Task<object> GetSummaryReportAsync()
        {
            return await _repository.GetSummaryReportAsync();
        }

        // Users report
        public async Task<object> GetUserReportAsync(ReportFilterDto filter)
        {
            return await _repository.GetUserReportAsync(filter);
        }

        // Artworks report
        public async Task<object> GetArtworkReportAsync(ReportFilterDto filter)
        {
            return await _repository.GetArtworksReportAsync(filter);
        }

        // Sales report
        public async Task<object> GetSalesReportAsync(ReportFilterDto filter)
        {
            return await _repository.GetSalesReportAsync(filter);
        }
    }
}
