// ========================= IAdminReportService.cs =========================
using System.Collections.Generic;
using System.Threading.Tasks;
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Services.Interfaces
{
    public interface IAdminReportService
    {
        Task<object> GetSummaryReportAsync();
        Task<object> GetUserReportAsync(ReportFilterDto filter);
        Task<object> GetArtworkReportAsync(ReportFilterDto filter);
        Task<object> GetSalesReportAsync(ReportFilterDto filter);
    }
}
