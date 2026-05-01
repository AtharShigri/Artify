using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using artifi.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace artifi.Api.Controllers.Buyer
{
    [Route("api/buyer/payment")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid) ? guid : null;
        }

        /// <summary>
        /// Initiates the Escrow process (Buyer pays platform)
        /// </summary>
        [HttpPost("initiate-escrow")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> InitiateEscrow([FromQuery] Guid orderId, [FromBody] decimal amount)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                // This creates the record with 10% commission held
                var escrow = await _paymentService.CreateEscrowRecordAsync(orderId, amount);
                return Ok(new { 
                    message = "Payment held in escrow successfully", 
                    escrowId = escrow.Id,
                    status = escrow.Status 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating escrow for Order {OrderId}", orderId);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Buyer confirms service received - Releases money from Escrow to Artist
        /// </summary>
        [HttpPost("confirm-receipt/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmReceipt(Guid orderId)
        {
            try
            {
                var result = await _paymentService.ReleasePaymentToArtistAsync(orderId);
                if (!result)
                    return BadRequest(new { message = "Unable to release payment. Check order status." });

                return Ok(new { message = "Payment released to artist successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error releasing payment for Order {OrderId}", orderId);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get escrow/payment status for an order
        /// </summary>
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(Guid orderId)
        {
            try
            {
                var status = await _paymentService.GetPaymentStatusAsync(orderId);
                return Ok(new { orderId, status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for Order {OrderId}", orderId);
                return StatusCode(500, new { message = "An error occurred fetching status" });
            }
        }

        /// <summary>
        /// Process refund back to buyer if service fails
        /// </summary>
        [HttpPost("refund/{orderId}")]
        public async Task<IActionResult> ProcessRefund(Guid orderId)
        {
            try
            {
                var result = await _paymentService.RefundPaymentToBuyerAsync(orderId);
                if (!result)
                    return BadRequest(new { message = "Refund failed or not applicable." });

                return Ok(new { message = "Refund processed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for Order {OrderId}", orderId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}