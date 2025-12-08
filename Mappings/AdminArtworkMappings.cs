// ========================= AdminArtworkMappings.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public static class AdminArtworkMappings
    {
        public static object ToAdminArtworkDto(Artwork artwork)
        {
            return new
            {
                artwork.ArtworkId,
                artwork.Title,
                artwork.Description,
                artwork.CategoryId,
                artwork.Price,
                artwork.Status,
                artwork.ArtistProfileId,
                artwork.CreatedAt
            };
        }
    }
}
