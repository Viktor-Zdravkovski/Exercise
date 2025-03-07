using BasicWeb.Database.Context;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicWeb.Database.Implementations
{
    public class CompanyRepository : IRepository<Company>
    {
        private readonly BasicWebDbContext _context;

        public CompanyRepository(BasicWebDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Company>> Get()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<int> Create(Company entity)
        {
            _context.Companies.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Update(Company entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Company entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<Company> GetById(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
