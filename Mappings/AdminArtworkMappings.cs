// ========================= AdminArtworkMappings.cs =========================
using artifi.Api.DTOs.Admin;
using artifi.Api.Models;

namespace artifi.Api.Mappings
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
                artwork.ImageUrl,
                artwork.CreatedAt
            };
        }
    }
}
