using Event_Management.BLL.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Services
{
    public interface IEventService
    {
        Task<EventDto> CreateEvent(CreateEventRequest request, int organizerId);
        Task<EventDto> GetEventById(int eventId);
        Task<List<EventDto>> GetAllPublishedEvents();
        Task<List<EventDto>> GetOrganizerEvents(int organizerId);
        Task<EventDto> UpdateEvent(int eventId, CreateEventRequest request, int organizerId);
        Task DeleteEvent(int eventId, int organizerId);
        Task PublishEvent(int eventId, int organizerId);
        Task CancelEvent(int eventId, int organizerId);

    }
}
