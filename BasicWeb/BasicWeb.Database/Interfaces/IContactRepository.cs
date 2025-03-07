using BasicWeb.Domain;

namespace BasicWeb.Database.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<List<Contact>> GetContactsWithCompanyAndCountry();

        Task<List<Contact>> FilterContacts(int countryId, int companyId);

        Task<bool> IfCompanyExists(int companyId);

        Task<bool> IfCountryExists(int countryId);
    }
}
