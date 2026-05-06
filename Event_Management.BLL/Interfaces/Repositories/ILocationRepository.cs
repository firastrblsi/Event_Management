using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface ILocationRepository
    {
        Task<Location?> GetByIdAsync(int id);
        Task<List<Location>> GetAllAsync();
        Task AddAsync(Location location);
    }
}
