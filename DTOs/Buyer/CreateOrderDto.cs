namespace Artify.Api.DTOs.Buyer
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Stripe";
    }

    public class OrderItemDto
    {
        public Guid ArtworkId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string DeliveryStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public Guid ArtworkId { get; set; }
        public string ArtworkTitle { get; set; } = string.Empty;
        public string ArtworkImage { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}