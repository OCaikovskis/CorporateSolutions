using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Seeders
{
    public class ProductSeeder : IDbSeeder
    {
        private readonly ILogger<ProductSeeder> _logger;

        public ProductSeeder(ILogger<ProductSeeder> logger)
        {
            _logger = logger;
        }

        public async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            if (!await context.Products.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Seeding initial messages...");

                context.Products.Add(new Product
                {
                    Price = 19.99m,
                    Quantity = 100,
                    Title = "Sample Product"
                });

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