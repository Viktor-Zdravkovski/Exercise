using BasicWeb.Domain;

namespace BasicWeb.Database.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country> GetCountryWithContacts(int id);
    }
}
