namespace Artify.Api.DTOs.Buyer
{
    public class CartItemDto
    {
        public int ArtworkId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemDto
    {
        public int Quantity { get; set; }
    }

    public class CartResponseDto
    {
        public string BuyerId { get; set; } = string.Empty;
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
        public int TotalItems => Items.Sum(i => i.Quantity);
    }

    public class CartItemResponseDto
    {
        public int CartItemId { get; set; }
        public int ArtworkId { get; set; }
        public string ArtworkTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public bool IsAvailable { get; set; } = true;
    }
}