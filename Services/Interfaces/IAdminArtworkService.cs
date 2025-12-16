// ========================= IAdminArtworkService.cs =========================
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
        Task<object?> GetArtworkByIdAsync(int artworkId);
        Task<object> ApproveArtworkAsync(int artworkId);
        Task<object> RejectArtworkAsync(int artworkId, ArtworkModerationDto dto);
        Task<bool> RemoveArtworkAsync(int artworkId);
    }
}
