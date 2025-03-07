using AutoMapper;
using BasicWeb.Database.Implementations;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Dto.CountryDto;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.Extensions.Logging;

namespace BasicWeb.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly ILogger<CountryService> _logger;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public CountryService(ILogger<CountryService> logger, ICountryRepository countryRepo, IMapper mapper)
        {
            _logger = logger;
            _countryRepo = countryRepo;
            _mapper = mapper;
        }

        public async Task<int> AddCountry(AddCountryDto addCountryDto)
        {
            if (string.IsNullOrWhiteSpace(addCountryDto.Name))
            {
                throw new ArgumentException("Country name cannot be empty.");
            }

            if (addCountryDto.Name.All(char.IsDigit))
            {
                throw new ArgumentException("Country name cannot contain only numbers.");
            }

            Country country = _mapper.Map<Country>(addCountryDto);
            await _countryRepo.Create(country);
            return country.Id;
        }

        public async Task<int> UpdateCountry(UpdateCountryDto updateCountryDto)
        {
            if (string.IsNullOrWhiteSpace(updateCountryDto.Name))
            {
                throw new ArgumentException("Country name cannot be empty");
            }

            if (updateCountryDto.Name.All(char.IsDigit))
            {
                throw new ArgumentException("Company name cannot contain only numbers");
            }

            Country existingCountry = _countryRepo.GetById(updateCountryDto.Id);

            if (existingCountry == null)
            {
                _logger.LogError("Update Failed: A country was not found");
                throw new NotFoundException("No country found to update");
            }

            existingCountry.Name = updateCountryDto.Name;

            await _countryRepo.Update(existingCountry);
            return existingCountry.Id;
        }

        public void DeleteCountry(int id)
        {
            Country country = _countryRepo.GetById(id);

            if (country == null)
            {
                _logger.LogError($"Delete failed: Country with id: {id} doesnt exists");
                throw new NotFoundException($"Country with ID: {id} was not found");
            }

            _countryRepo.Delete(country);
        }

        public async Task<List<CountryDto>> GetAll()
        {
            IReadOnlyList<Country> countries = await _countryRepo.Get();

            if (countries == null || !countries.Any())
            {
                throw new NotFoundException("No countries found.");
            }

            return _mapper.Map<List<CountryDto>>(countries);
        }

        public async Task<Dictionary<string, int>> GetCompanyStatisticsByCountryId(int countryId)
        {
            Country countryWithContacts = await _countryRepo.GetCountryWithContacts(countryId);

            if (countryWithContacts == null)
            {
                _logger.LogError($"Failed to find a country with ID: {countryId}");
                throw new NotFoundException($"No country found with ID: {countryId}");
            }

            return countryWithContacts.GetCompanyStatistics();
        }
    }
}