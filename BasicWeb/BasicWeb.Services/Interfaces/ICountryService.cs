using BasicWeb.Dto.CountryDto;

namespace BasicWeb.Services.Interfaces
{
    public interface ICountryService
    {
        Task<int> AddCountry(AddCountryDto addCountryDto);

        Task<int> UpdateCountry(UpdateCountryDto updateCountryDto);

        Task DeleteCountry(int id);

        Task<List<CountryDto>> GetAll();

        Task<Dictionary<string, int>> GetCompanyStatisticsByCountryId(int countryId);
    }
}
