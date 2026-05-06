using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IOrganizerRepository
    {
        Task AddAsync(Organizer organizer);
        Task<Organizer?> GetByUserIdAsync(int userId);
    }
}
