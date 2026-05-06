using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdWithIncludesAsync(int id);
        Task<List<Booking>> GetByUserIdAsync(int userId);
        Task UpdateAsync(Booking booking);
    }
}
