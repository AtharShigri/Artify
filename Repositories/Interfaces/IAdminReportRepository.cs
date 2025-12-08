// ========================= IAdminReportRepository.cs =========================
using Artify.Api.DTOs.Admin;
using System.Threading.Tasks;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IAdminReportRepository
    {
        Task<object> GetSummaryReportAsync();
        Task<object> GetUserReportAsync(ReportFilterDto filter);
        Task<object> GetArtworksReportAsync(ReportFilterDto filter);
        Task<object> GetSalesReportAsync(ReportFilterDto filter);
    }
}
