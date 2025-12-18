using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
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

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Create payment intent
        /// </summary>
        [HttpPost("create-intent")]
        [ProducesResponseType(typeof(PaymentResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentDto paymentIntentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var paymentIntent = await _paymentService.CreatePaymentIntentAsync(paymentIntentDto.OrderId, buyerId);
                return Ok(paymentIntent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Confirm payment
        /// </summary>
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(PaymentResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmDto confirmDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var payment = await _paymentService.ConfirmPaymentAsync(confirmDto.PaymentIntentId, confirmDto.OrderId);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process payment webhook (no authentication required for webhooks)
        /// </summary>
        [HttpPost("webhook")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ProcessWebhook([FromBody] PaymentCallbackDto webhookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _paymentService.ProcessPaymentWebhookAsync(webhookDto);
                if (!result)
                    return BadRequest(new { message = "Failed to process webhook" });

                return Ok(new { message = "Webhook processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment webhook");
                return StatusCode(500, new { message = "An error occurred while processing webhook" });
            }
        }

        /// <summary>
        /// Get payment status for an order
        /// </summary>
        [HttpGet("status/{orderId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPaymentStatus(Guid orderId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var status = await _paymentService.GetPaymentStatusAsync(orderId);
                return Ok(new { status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status");
                return StatusCode(500, new { message = "An error occurred while fetching payment status" });
            }
        }

        /// <summary>
        /// Get transaction history for buyer
        /// </summary>
        [HttpGet("transactions")]
        [ProducesResponseType(typeof(IEnumerable<TransactionLogDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var transactions = await _paymentService.GetBuyerTransactionsAsync(buyerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions");
                return StatusCode(500, new { message = "An error occurred while fetching transactions" });
            }
        }

        /// <summary>
        /// Process refund for an order
        /// </summary>
        [HttpPost("refund/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ProcessRefund(Guid orderId, [FromBody] decimal amount)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _paymentService.ProcessRefundAsync(orderId, amount);
                if (!result)
                    return BadRequest(new { message = "Cannot process refund for this order" });

                return Ok(new { message = "Refund processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}