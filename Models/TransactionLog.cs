using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class TransactionLog
    {
        [Key]
        public Guid TransactionId { get; set; }
        public Guid BuyerId {  get; set; }
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]

        Guid? PaymentIntentId { get; set; }
        [ForeignKey("PaymentIntentId")]
        public Order Order { get; set; }

        public string PaymentMethod { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
    }
}
