using CorporateSolutions.Enums;
using CorporateSolutions.Models;
using CorporateSolutions.Seeders;

public class UserSeeder : IDbSeeder
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserSeeder(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
    {
        if (!_context.Users.Any())
        {
            var adminPassword = _configuration["AdminPassword"];
            var userPassword = _configuration["UserPassword"];

            var adminUser = new User
            {
                Username = "admin",
                Role = EUserRole.Admin,
                Password = BCrypt.Net.BCrypt.HashPassword(adminPassword)
            };

            var normalUser = new User
            {
                Username = "user",
                Role = EUserRole.User,
                Password = BCrypt.Net.BCrypt.HashPassword(userPassword)
            };

            _context.Users.AddRange(adminUser, normalUser);
            _context.SaveChanges();
        }
    }
}
