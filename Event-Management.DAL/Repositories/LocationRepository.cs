using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.DAL.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext _context;

        public LocationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations
                .OrderBy(l => l.Name)
                .ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }
    }
}