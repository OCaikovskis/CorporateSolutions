using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Repositories
{
    public interface IAuditRepository
    {
        Task<List<Audit>> GetAuditAsync(DateTime? from, DateTime? to);
        Task LogAuditAsync(Audit audit);
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Audit>> GetAuditAsync(DateTime? from, DateTime? to)
        {
            var query = _context.Audits.AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(a => a.Timestamp >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(a => a.Timestamp <= to.Value);
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task LogAuditAsync(Audit audit)
        {
            await _context.Audits.AddAsync(audit);
            await _context.SaveChangesAsync();
        }
    }
}