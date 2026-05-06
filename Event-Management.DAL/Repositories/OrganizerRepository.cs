using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.DAL.Repositories
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly DataContext _context;

        public OrganizerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Organizer organizer)
        {
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();
        }

        public async Task<Organizer?> GetByUserIdAsync(int userId)
        {
            return await _context.Organizers.FirstOrDefaultAsync(o => o.UserId == userId);
        }
    }
}
