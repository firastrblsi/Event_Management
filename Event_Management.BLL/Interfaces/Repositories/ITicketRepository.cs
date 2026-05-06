using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket?> GetByIdForEventAsync(int ticketId, int eventId);
        Task UpdateAsync(Ticket ticket);
        Task AddAsync(Ticket ticket);
    }
}
