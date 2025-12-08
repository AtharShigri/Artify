// ========================= AdminTransactionMappings.cs =========================
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public static class AdminTransactionMappings
    {
        public static object ToTransactionDto(TransactionLog transaction)
        {
            return new
            {
                transaction.TransactionId,
                transaction.OrderId,
                transaction.TransactionAmount,
                transaction.Status,
                transaction.PaymentMethod,
                transaction.TransactionDate
            };
        }

        public static object ToOrderDto(Order order)
        {
            return new
            {
                order.OrderId,
                order.ArtworkId,
                order.BuyerId,
                order.TotalAmount,
                order.DeliveryStatus,
                order.CreatedAt
            };
        }
    }
}
