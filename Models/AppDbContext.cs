using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Message> Messages { get; set; }
    }
}
