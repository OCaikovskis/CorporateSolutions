using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Repositories
{
    public interface IMessageRepository
    {
        Task<Message?> GetFirstMessageAsync();
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetFirstMessageAsync()
        {
            return await _context.Messages
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}