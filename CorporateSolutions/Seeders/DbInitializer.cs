using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Seeders
{
    public static class DbInitializer
    {
        public static async Task InitialiseAsync(IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using var scope = services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
            var seeders = scope.ServiceProvider.GetServices<IDbSeeder>();

            try
            {
                logger.LogInformation("Applying database migrations...");
                await dbContext.Database.MigrateAsync(cancellationToken);
                logger.LogInformation("Migrations applied successfully.");

                foreach (var seeder in seeders)
                {
                    logger.LogInformation("Running seeder {SeederType}...", seeder.GetType().Name);
                    await seeder.SeedAsync(dbContext, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}
