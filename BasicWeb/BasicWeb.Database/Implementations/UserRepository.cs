using BasicWeb.Database.Context;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicWeb.Database.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly BasicWebDbContext _context;

        public UserRepository(BasicWebDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(User id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }


        public async Task<int> Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<User> LogInUser(string email, string hashPasswrod)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == hashPasswrod);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }
    }
}
