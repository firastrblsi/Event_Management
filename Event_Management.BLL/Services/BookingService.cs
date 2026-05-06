using Event_Management.BLL.DTOs.Booking;
using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Event_Management.CrossCutting.Enums;

namespace Event_Management.BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IEventRepository eventRepository,
            ITicketRepository ticketRepository)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<BookingDto> CreateBooking(CreateBookingRequest request, int userId)
        {
            var @event = await _eventRepository.GetByIdWithIncludesAsync(request.EventId);
            if (@event == null)
                throw new InvalidOperationException("Event not found");

            if (@event.Status == EventStatus.Cancelled)
                throw new InvalidOperationException("Cannot book cancelled events");

            var ticket = await _ticketRepository.GetByIdForEventAsync(request.TicketId, request.EventId);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found");

            var availableTickets = ticket.QuantityAvailable - ticket.QuantitySold;
            if (availableTickets < request.Quantity)
                throw new InvalidOperationException($"Only {availableTickets} tickets available");

            var booking = new Booking
            {
                UserId = userId,
                EventId = request.EventId,
                TicketId = request.TicketId,
                Quantity = request.Quantity,
                Status = BookingStatus.Pending
            };

            // update ticket sold count first (atomicity: repositories persist immediately)
            ticket.QuantitySold += request.Quantity;
            await _ticketRepository.UpdateAsync(ticket);

            await _bookingRepository.AddAsync(booking);

            var created = await _bookingRepository.GetByIdWithIncludesAsync(booking.Id);
            return MapToDto(created!);
        }

        public async Task<BookingDto> GetBooking(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdWithIncludesAsync(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only view your own bookings");

            return MapToDto(booking);
        }

        public async Task<List<BookingDto>> GetUserBookings(int userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return bookings.Select(MapToDto).ToList();
        }

        public async Task CancelBooking(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdWithIncludesAsync(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own bookings");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled");

            booking.Status = BookingStatus.Cancelled;
            booking.CancelledAt = DateTime.UtcNow;

            var ticket = booking.Ticket;
            ticket.QuantitySold -= booking.Quantity;

            await _ticketRepository.UpdateAsync(ticket);
            await _bookingRepository.UpdateAsync(booking);
        }

        private BookingDto MapToDto(Booking booking) => new BookingDto
        {
            Id = booking.Id,
            EventId = booking.EventId,
            EventTitle = booking.Event.Title,
            Quantity = booking.Quantity,
            Status = booking.Status.ToString(),
            BookingDate = booking.BookingDate,
            TotalPrice = booking.Ticket.Price * booking.Quantity
        };
    }
}