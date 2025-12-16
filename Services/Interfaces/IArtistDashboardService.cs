using System.Security.Claims;

namespace Artify.Api.Services.Interfaces
{
    public interface IArtistDashboardService
    {
        Task<object> GetSummaryAsync(ClaimsPrincipal user);
        Task<object> GetOrdersAsync(ClaimsPrincipal user);
        Task<object> GetReviewsAsync(ClaimsPrincipal user);
    }
}
