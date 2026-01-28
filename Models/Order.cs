using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public ApplicationUser Buyer { get; set; }

        public Guid? ArtworkId { get; set; }
        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }

        public Guid ArtistProfileId { get; set; }
        [ForeignKey("ArtistProfileId")]
        public ArtistProfile ArtistProfile { get; set; }

        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderType { get; set; } // Artwork Purchase / Service Hire
        public string DeliveryStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
