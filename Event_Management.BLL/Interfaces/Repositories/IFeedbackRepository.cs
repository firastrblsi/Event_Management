using Event_Management.CrossCutting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.BLL.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        Task AddAsync(Feedback feedback);
        Task<Feedback?> GetByUserAndEventAsync(int userId, int eventId);
        Task<List<Feedback>> GetByEventAsync(int eventId);
    }
}
