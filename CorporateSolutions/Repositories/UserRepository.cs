using CorporateSolutions.Classes;
using CorporateSolutions.Exceptions;
using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(LoginRequest request);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(LoginRequest request)
        {
            var user = await _context.Users
                 .AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                throw new ApiException("Unautherised", "Invalid username or password");
            }

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isValid)
            {
                throw new ApiException("Unautherised", "Invalid username or password");
            }

            return user;
        }
    }
}