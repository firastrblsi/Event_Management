using Event_Management.BLL.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreatePayment(CreatePaymentRequest request, int userId);
        Task<PaymentDto> GetPaymentByBooking(int bookingId, int userId);
        Task<PaymentDto> UpdatePaymentStatus(int bookingId, string newStatus, int userId);
    }
}
