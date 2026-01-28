using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminTransactionsController : ControllerBase
    {
        private readonly IAdminTransactionService _service;

        public AdminTransactionsController(IAdminTransactionService service)
        {
            _service = service;
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterDto dto)
        {
            var result = await _service.GetTransactionsAsync(dto);
            return Ok(result);
        }

        [HttpGet("transactions/{transactionId}")]
        public async Task<IActionResult> GetTransaction(Guid transactionId)
        {
            var result = await _service.GetTransactionByIdAsync(transactionId);
            return Ok(result);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _service.GetOrdersAsync();
            return Ok(result);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var result = await _service.GetOrderByIdAsync(orderId);
            return Ok(result);
        }
    }
}
