using Event_Management.BLL.DTOs.Feedback;
using Event_Management.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedback;

        public FeedbackController(IFeedbackService feedback) => _feedback = feedback;

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User id not found"));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackRequest request)
        {
            var userId = GetUserId();
            var created = await _feedback.CreateFeedback(request, userId);
            return CreatedAtAction(nameof(GetForEvent), new { eventId = created.EventId }, created);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            var list = await _feedback.GetEventFeedbacks(eventId);
            return Ok(list);
        }
    }
}