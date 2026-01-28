namespace Artify.Api.DTOs.Buyer
{
    public class PaymentIntentDto
    {
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; } = "Stripe";
    }

    public class PaymentConfirmDto
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
    }

    public class PaymentCallbackDto
    {
        public string EventType { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
    }

    public class PaymentResponseDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}