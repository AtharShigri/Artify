using Artify.Api.Enums;

namespace Artify.Api.Models
{
    public class HiringRequest
    {
        public Guid Id { get; set; }

        public Guid ArtistProfileId { get; set; }
        public Guid BuyerId { get; set; }

        public string Description { get; set; }
        public HiringStatus Status { get; set; } = HiringStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
