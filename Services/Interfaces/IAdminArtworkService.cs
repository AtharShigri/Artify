using Artify.Api.DTOs.Admin;
using Artify.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artify.Api.Services.Interfaces
{
    public interface IAdminArtworkService
    {
        Task<IEnumerable<object>> GetAllArtworksAsync();
        Task<IEnumerable<object>> GetPendingArtworksAsync();
        Task<object?> GetArtworkByIdAsync(Guid artworkId);
        Task<object> ApproveArtworkAsync(Guid artworkId);
        Task<object> RejectArtworkAsync(Guid artworkId, ArtworkModerationDto dto);
        Task<bool> RemoveArtworkAsync(Guid artworkId);
    }
}
