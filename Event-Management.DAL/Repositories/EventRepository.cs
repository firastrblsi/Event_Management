using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.DAL.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;

        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Organizer).ThenInclude(o => o.User)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Tickets)
                .Include(e => e.Bookings)
                .Include(e => e.Feedbacks)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetPublishedAsync()
        {
            return await _context.Events
                .Where(e => e.Status == Event_Management.CrossCutting.Enums.EventStatus.Published)
                .Include(e => e.Organizer).ThenInclude(o => o.User)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Event>> GetByOrganizerUserIdAsync(int organizerUserId)
        {
            return await _context.Events
                .Where(e => e.Organizer.UserId == organizerUserId)
                .Include(e => e.Organizer).ThenInclude(o => o.User)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event @event)
        {
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
        }
    }
}
