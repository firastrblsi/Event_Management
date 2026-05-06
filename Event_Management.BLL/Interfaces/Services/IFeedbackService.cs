using Event_Management.BLL.DTOs.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> CreateFeedback(CreateFeedbackRequest request, int userId);
        Task<List<FeedbackDto>> GetEventFeedbacks(int eventId);
    }
}
