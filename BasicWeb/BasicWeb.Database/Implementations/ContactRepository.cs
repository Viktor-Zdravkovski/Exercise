using System.ComponentModel.Design;
using BasicWeb.Database.Context;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicWeb.Database.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly BasicWebDbContext _context;

        public ContactRepository(BasicWebDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Contact>> Get()
        {
            return await _context.Contacts.Include(x => x.Country)
                                          .Include(x => x.Company)
                                          .ToListAsync();
        }

        public async Task<int> Create(Contact entity)
        {
            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Update(Contact entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Contact id)
        {
            _context.Remove(id);
            _context.SaveChanges();
        }

        public async Task<Contact> GetById(int id)
        {
            return await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Contact>> GetContactsWithCompanyAndCountry()
        {
            return await _context.Contacts.Include(x => x.Company)
                                          .Include(x => x.Country)
                                          .ToListAsync();
        }

        public async Task<List<Contact>> FilterContacts(int countryId, int companyId)
        {
            return await _context.Contacts.Where(x => x.CountryId == countryId || x.CompanyId == companyId)
                                          .Include(x => x.Company)
                                          .Include(x => x.Country)
                                          .ToListAsync();
        }

        public async Task<bool> IfCompanyExists(int companyId)
        {
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public async Task<bool> IfCountryExists(int countryId)
        {
            return await _context.Countries.AnyAsync(x => x.Id == countryId);
        }
    }
}