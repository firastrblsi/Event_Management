using Event_Management.BLL.DTOs.Payment;
using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;
using Event_Management.CrossCutting.Enums;

namespace Event_Management.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<PaymentDto> CreatePayment(CreatePaymentRequest request, int userId)
        {
            var booking = await _bookingRepository.GetByIdWithIncludesAsync(request.BookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only pay for your own bookings");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Cannot pay for a cancelled booking");

            var expected = booking.Ticket.Price * booking.Quantity;
            if (request.Amount != expected)
                throw new InvalidOperationException("Payment amount does not match booking total");

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status.ToString(),
                PaidAt = payment.PaidAt
            };
        }

        public async Task<PaymentDto> GetPaymentByBooking(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdWithIncludesAsync(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only view payments for your own bookings");

            var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
            if (payment == null)
                throw new InvalidOperationException("Payment not found");

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status.ToString(),
                PaidAt = payment.PaidAt
            };
        }

        public async Task<PaymentDto> UpdatePaymentStatus(int bookingId, string newStatus, int userId)
        {
            // Typically only payment provider/webhook or admin updates status.
            var booking = await _bookingRepository.GetByIdWithIncludesAsync(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            // Allow user only if they own booking; otherwise service can be called by admin (not enforced here)
            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only update payments for your own bookings");

            var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
            if (payment == null)
                throw new InvalidOperationException("Payment not found");

            if (!Enum.TryParse<PaymentStatus>(newStatus, true, out var status))
                throw new InvalidOperationException("Invalid payment status");

            payment.Status = status;
            if (status == PaymentStatus.Completed)
                payment.PaidAt = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);

            return new PaymentDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status.ToString(),
                PaidAt = payment.PaidAt
            };
        }
    }
}