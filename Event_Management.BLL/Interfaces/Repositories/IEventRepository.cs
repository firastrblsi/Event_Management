using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task AddAsync(Event @event);
        Task<Event?> GetByIdAsync(int id);
        Task<Event?> GetByIdWithIncludesAsync(int id);
        Task<List<Event>> GetPublishedAsync();
        Task<List<Event>> GetByOrganizerUserIdAsync(int organizerUserId);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(Event @event);
    }
}
