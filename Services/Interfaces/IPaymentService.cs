using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artify.Api.Models;

namespace Artify.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        // Core Escrow Operations
        Task<EscrowTransaction> CreateEscrowRecordAsync(Guid orderId, decimal totalAmount);
        Task<bool> ReleasePaymentToArtistAsync(Guid orderId);
        Task<bool> RefundPaymentToBuyerAsync(Guid orderId);
        
        // Math Logic
        decimal CalculateCommission(decimal totalAmount);
        
        // Status & History
        Task<string> GetPaymentStatusAsync(Guid orderId);
    }
}