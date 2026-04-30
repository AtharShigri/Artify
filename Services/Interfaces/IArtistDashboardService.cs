using System.Security.Claims;

namespace artifi.Api.Services.Interfaces
{
    public interface IArtistDashboardService
    {
        Task<object> GetSummaryAsync(ClaimsPrincipal user);
        Task<object> GetOrdersAsync(ClaimsPrincipal user);
        Task<object> GetReviewsAsync(ClaimsPrincipal user);
        Task<object> GetEarningsAsync(ClaimsPrincipal user);
        Task<object> GetArtworkStatsAsync(ClaimsPrincipal user);

    }
}
