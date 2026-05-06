using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;

namespace Event_Management.BLL.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<Location> CreateLocation(Location location)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(location.Name))
                throw new InvalidOperationException("Location name is required");

            await _locationRepository.AddAsync(location);
            return location;
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }
    }
}