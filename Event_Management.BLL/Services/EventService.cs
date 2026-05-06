using Event_Management.BLL.DTOs.Event;
using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Event_Management.CrossCutting.Enums;

namespace Event_Management.BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly ICategoryRepository? _categoryRepository;
        private readonly ILocationRepository? _locationRepository;

        public EventService(
            IEventRepository eventRepository,
            IOrganizerRepository organizerRepository,
            ICategoryRepository? categoryRepository,
            ILocationRepository? locationRepository)
        {
            _eventRepository = eventRepository;
            _organizerRepository = organizerRepository;
            _categoryRepository = categoryRepository ?? categoryRepository;
            _locationRepository = locationRepository;
        }

        public async Task<EventDto> CreateEvent(CreateEventRequest request, int organizerId)
        {
            var organizer = await _organizerRepository.GetByUserIdAsync(organizerId);
            if (organizer == null)
                throw new InvalidOperationException("Organizer profile not found");

            var category = await _categoryRepository!.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new InvalidOperationException("Category not found");

            var location = await _locationRepository!.GetByIdAsync(request.LocationId);
            if (location == null)
                throw new InvalidOperationException("Location not found");

            var @event = new Event
            {
                Title = request.Title,
                Description = request.Description,
                EventDate = request.EventDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = EventStatus.Draft,
                OrganizerId = organizer.Id,
                CategoryId = request.CategoryId,
                LocationId = request.LocationId
            };

            await _eventRepository.AddAsync(@event);

            var created = await _eventRepository.GetByIdWithIncludesAsync(@event.Id);
            return MapToDto(created!);
        }

        public async Task<EventDto> GetEventById(int eventId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            return MapToDto(@event);
        }

        public async Task<List<EventDto>> GetAllPublishedEvents()
        {
            var events = await _eventRepository.GetPublishedAsync();
            return events.Select(MapToDto).ToList();
        }

        public async Task<List<EventDto>> GetOrganizerEvents(int organizerId)
        {
            var events = await _eventRepository.GetByOrganizerUserIdAsync(organizerId);
            return events.Select(MapToDto).ToList();
        }

        public async Task<EventDto> UpdateEvent(int eventId, CreateEventRequest request, int organizerId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            if (@event.Organizer.UserId != organizerId)
                throw new UnauthorizedAccessException("You can only update your own events");

            if (@event.Status == EventStatus.Published)
                throw new InvalidOperationException("Cannot modify published events");

            @event.Title = request.Title;
            @event.Description = request.Description;
            @event.EventDate = request.EventDate;
            @event.StartTime = request.StartTime;
            @event.EndTime = request.EndTime;
            @event.CategoryId = request.CategoryId;
            @event.LocationId = request.LocationId;
            @event.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(@event);

            var updated = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            return MapToDto(updated!);
        }

        public async Task DeleteEvent(int eventId, int organizerId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            if (@event.Organizer.UserId != organizerId)
                throw new UnauthorizedAccessException("You can only delete your own events");

            await _eventRepository.DeleteAsync(@event);
        }

        public async Task PublishEvent(int eventId, int organizerId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            if (@event.Organizer.UserId != organizerId)
                throw new UnauthorizedAccessException("You can only publish your own events");

            if (!@event.Tickets.Any())
                throw new InvalidOperationException("Event must have at least one ticket before publishing");

            @event.Status = EventStatus.Published;
            @event.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(@event);
        }

        public async Task CancelEvent(int eventId, int organizerId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(eventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            if (@event.Organizer.UserId != organizerId)
                throw new UnauthorizedAccessException("You can only cancel your own events");

            @event.Status = EventStatus.Cancelled;
            @event.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(@event);
        }

        private EventDto MapToDto(Event @event) => new EventDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            EventDate = @event.EventDate,
            StartTime = @event.StartTime,
            EndTime = @event.EndTime,
            Status = @event.Status.ToString(),
            OrganizerName = @event.Organizer.User.Name,
            Category = @event.Category.Name,
            Location = @event.Location.Name
        };
    }
}