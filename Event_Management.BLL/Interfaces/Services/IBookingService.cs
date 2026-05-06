using Event_Management.BLL.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBooking(CreateBookingRequest request, int userId);
        Task<BookingDto> GetBooking(int bookingId, int userId);
        Task<List<BookingDto>> GetUserBookings(int userId);
        Task CancelBooking(int bookingId, int userId);
    }
}
