using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Seeders
{
    public class MessageSeeder : IDbSeeder
    {
        private readonly ILogger<MessageSeeder> _logger;

        public MessageSeeder(ILogger<MessageSeeder> logger)
        {
            _logger = logger;
        }

        public async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            if (!await context.Messages.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Seeding initial messages...");

                context.Messages.Add(new Message { Text = "hello world" });
                await context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Seeding complete.");
            }
            else
            {
                _logger.LogInformation("Messages already exist, skipping seeding.");
            }
        }
    }
}