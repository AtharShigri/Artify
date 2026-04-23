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
    public virtual ApplicationUser Buyer { get; set; } = null!;

    public Guid? ArtworkId { get; set; }
    [ForeignKey("ArtworkId")]
    public virtual Artwork? Artwork { get; set; }

    // ADD THIS: To track if they hired a service
    public Guid? ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public virtual Service? Service { get; set; }

    public Guid ArtistProfileId { get; set; }
    [ForeignKey("ArtistProfileId")]
    public virtual ArtistProfile ArtistProfile { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public string OrderType { get; set; } // "Artwork" or "Service"
    public string DeliveryStatus { get; set; } = "Processing";
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletionDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}
