using Event_Management.BLL.DTOs.Payment;
using Event_Management.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payments;

        public PaymentController(IPaymentService payments) => _payments = payments;

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User id not found"));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var userId = GetUserId();
            var payment = await _payments.CreatePayment(request, userId);
            return CreatedAtAction(nameof(GetByBooking), new { bookingId = payment.BookingId }, payment);
        }

        [HttpGet("booking/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> GetByBooking(int bookingId)
        {
            var userId = GetUserId();
            var payment = await _payments.GetPaymentByBooking(bookingId, userId);
            return Ok(payment);
        }

        [HttpPut("booking/{bookingId}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int bookingId, [FromBody] string newStatus)
        {
            var userId = GetUserId();
            var updated = await _payments.UpdatePaymentStatus(bookingId, newStatus, userId);
            return Ok(updated);
        }
    }
}