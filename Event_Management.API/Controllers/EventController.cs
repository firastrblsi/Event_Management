using Event_Management.BLL.DTOs.Event;
using Event_Management.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _events;

        public EventController(IEventService events) => _events = events;

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User id not found"));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            var organizerId = GetUserId();
            var created = await _events.CreateEvent(request, organizerId);
            return CreatedAtAction(nameof(GetEventById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var ev = await _events.GetEventById(id);
            return Ok(ev);
        }

        [HttpGet("published")]
        public async Task<IActionResult> GetAllPublished()
        {
            var list = await _events.GetAllPublishedEvents();
            return Ok(list);
        }

        [HttpGet("organizer")]
        [Authorize]
        public async Task<IActionResult> GetOrganizerEvents()
        {
            var organizerId = GetUserId();
            var list = await _events.GetOrganizerEvents(organizerId);
            return Ok(list);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] CreateEventRequest request)
        {
            var organizerId = GetUserId();
            var updated = await _events.UpdateEvent(id, request, organizerId);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var organizerId = GetUserId();
            await _events.DeleteEvent(id, organizerId);
            return NoContent();
        }

        [HttpPost("{id}/publish")]
        [Authorize]
        public async Task<IActionResult> PublishEvent(int id)
        {
            var organizerId = GetUserId();
            await _events.PublishEvent(id, organizerId);
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelEvent(int id)
        {
            var organizerId = GetUserId();
            await _events.CancelEvent(id, organizerId);
            return NoContent();
        }
    }
}