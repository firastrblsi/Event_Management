using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByBookingIdAsync(int bookingId);
        Task UpdateAsync(Payment payment);
    }
}
