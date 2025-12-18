namespace Artify.Api.DTOs.Admin
{
    public class TransactionFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? SortBy { get; set; }
    }
}
