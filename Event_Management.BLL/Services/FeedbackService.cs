using Event_Management.BLL.DTOs.Feedback;
using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Event_Management.CrossCutting.Enums;

namespace Event_Management.BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IBookingRepository _bookingRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IBookingRepository bookingRepository)
        {
            _feedbackRepository = feedbackRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<FeedbackDto> CreateFeedback(CreateFeedbackRequest request, int userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            var bookingForEvent = bookings.FirstOrDefault(b => b.EventId == request.EventId && b.Status == BookingStatus.Confirmed);
            if (bookingForEvent == null)
                throw new InvalidOperationException("You must have a confirmed booking for this event to leave feedback");

            var existing = await _feedbackRepository.GetByUserAndEventAsync(userId, request.EventId);
            if (existing != null)
                throw new InvalidOperationException("You have already submitted feedback for this event");

            var feedback = new Feedback
            {
                UserId = userId,
                EventId = request.EventId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _feedbackRepository.AddAsync(feedback);

            return new FeedbackDto
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                EventId = feedback.EventId,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt,
                UserName = bookingForEvent.User?.Name ?? string.Empty
            };
        }

        public async Task<List<FeedbackDto>> GetEventFeedbacks(int eventId)
        {
            var list = await _feedbackRepository.GetByEventAsync(eventId);
            return list.Select(f => new FeedbackDto
            {
                Id = f.Id,
                UserId = f.UserId,
                UserName = f.User?.Name ?? string.Empty,
                EventId = f.EventId,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            }).ToList();
        }
    }
}