using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Services
{
    public interface ILocationService
    {
        Task<Location> CreateLocation(Location location);
        Task<Location?> GetByIdAsync(int id);
        Task<List<Location>> GetAllAsync();
    }
}
