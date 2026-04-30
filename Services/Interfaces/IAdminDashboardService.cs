using artifi.Api.DTOs.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace artifi.Api.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<RecentUserDto>> GetRecentUsersAsync(int count = 5);
        Task<IEnumerable<PlagiarismReportDto>> GetRecentPlagiarismReportsAsync(int count = 5);
    }
}
