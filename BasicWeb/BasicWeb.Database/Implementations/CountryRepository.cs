using BasicWeb.Database.Context;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicWeb.Database.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly BasicWebDbContext _context;

        public CountryRepository(BasicWebDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Country>> Get()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<int> Create(Country entity)
        {
            _context.Countries.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Update(Country entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Country id)
        {
            _context.Remove(id);
            _context.SaveChanges();
        }

        public async Task<Country> GetById(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Country> GetCountryWithContacts(int id)
        {
            return await _context.Countries.Include(x => x.Contacts).ThenInclude(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}