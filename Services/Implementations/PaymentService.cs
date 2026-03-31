using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Artify.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IEscrowRepository _escrowRepo;
        private const decimal CommissionRate = 0.10m; // 10% Platform Fee

        public PaymentService(IEscrowRepository escrowRepo)
        {
            _escrowRepo = escrowRepo;
        }

        public decimal CalculateCommission(decimal totalAmount) => totalAmount * CommissionRate;

        public async Task<EscrowTransaction> CreateEscrowRecordAsync(Guid orderId, decimal totalAmount)
        {
            var commission = CalculateCommission(totalAmount);
            var artistPayout = totalAmount - commission;

            var escrow = new EscrowTransaction
            {
                OrderId = orderId,
                TotalAmount = totalAmount,
                CommissionAmount = commission,
                ArtistPayoutAmount = artistPayout,
                Status = "Held", // Initial state
                CreatedAt = DateTime.UtcNow
            };

            await _escrowRepo.AddAsync(escrow);
            await _escrowRepo.SaveChangesAsync();
            return escrow;
        }

        public async Task<bool> ReleasePaymentToArtistAsync(Guid orderId)
        {
            var escrow = await _escrowRepo.GetByOrderIdAsync(orderId);
            if (escrow == null || escrow.Status != "Held") return false;

            escrow.Status = "Released";
            escrow.ReleasedAt = DateTime.UtcNow;

            await _escrowRepo.UpdateAsync(escrow);
            await _escrowRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefundPaymentToBuyerAsync(Guid orderId)
        {
            var escrow = await _escrowRepo.GetByOrderIdAsync(orderId);
            if (escrow == null || escrow.Status != "Held") return false;

            escrow.Status = "Refunded";
            await _escrowRepo.UpdateAsync(escrow);
            await _escrowRepo.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetPaymentStatusAsync(Guid orderId)
        {
            var escrow = await _escrowRepo.GetByOrderIdAsync(orderId);
            return escrow?.Status ?? "NotFound";
        }
    }
}