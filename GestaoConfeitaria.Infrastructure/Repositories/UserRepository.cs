using Microsoft.EntityFrameworkCore;
using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using GestaoConfeitaria.Infrastructure.Data;

namespace GestaoConfeitaria.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BoloDbContext _context;

        public UserRepository(BoloDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
