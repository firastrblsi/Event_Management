using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.CrossCutting.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.DAL.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly DataContext _context;

        public FeedbackRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task<Feedback?> GetByUserAndEventAsync(int userId, int eventId)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.UserId == userId && f.EventId == eventId);
        }

        public async Task<List<Feedback>> GetByEventAsync(int eventId)
        {
            return await _context.Feedbacks
                .Where(f => f.EventId == eventId)
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }
    }
}