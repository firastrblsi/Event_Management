using Event_Management.BLL.DTOs.Booking;
using Event_Management.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookings;

        public BookingController(IBookingService bookings) => _bookings = bookings;

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User id not found"));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            var userId = GetUserId();
            var booking = await _bookings.CreateBooking(request, userId);
            return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var userId = GetUserId();
            var booking = await _bookings.GetBooking(id, userId);
            return Ok(booking);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var userId = GetUserId();
            var list = await _bookings.GetUserBookings(userId);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetUserId();
            await _bookings.CancelBooking(id, userId);
            return NoContent();
        }
    }
}