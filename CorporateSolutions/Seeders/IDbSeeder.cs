using CorporateSolutions.Models;

namespace CorporateSolutions.Seeders
{
    public interface IDbSeeder
    {
        Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default);
    }
}
