using AutoMapper;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.CompanyDto;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Dto.CountryDto;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.Extensions.Logging;

namespace BasicWeb.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly ILogger<ContactService> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(ILogger<ContactService> logger, IContactRepository contactRepo, IMapper mapper)
        {
            _logger = logger;
            _contactRepository = contactRepo;
            _mapper = mapper;
        }

        public async Task<int> AddContact(AddContactDto addContactDto)
        {
            if (string.IsNullOrWhiteSpace(addContactDto.Name))
            {
                throw new ArgumentException("Contact name cannot be empty.");
            }

            if (addContactDto.Name.All(char.IsDigit))
            {
                throw new ArgumentException("Contact name cannot contain only numbers.");
            }

            if (addContactDto.CompanyId <= 0)
            {
                throw new ArgumentException("Invalid Company ID");
            }

            if (addContactDto.CountryId <= 0)
            {
                throw new ArgumentException("Invalid Country ID");
            }

            _logger.LogInformation($"Received CountryId: {addContactDto.CountryId}");

            bool existingCompany = await _contactRepository.IfCompanyExists(addContactDto.CompanyId);

            if (!existingCompany)
            {
                throw new ArgumentException($"Company with ID {addContactDto.CompanyId} does not exist.");
            }

            bool existingCountry = await _contactRepository.IfCountryExists(addContactDto.CountryId);

            if (!existingCountry)
            {
                throw new ArgumentException($"Country with ID {addContactDto.CountryId} does not exist.");
            }

            Contact contact = _mapper.Map<Contact>(addContactDto);
            await _contactRepository.Create(contact);
            return contact.Id;
        }

        public async Task<int> UpdateContact(UpdateContactDto updateContactDto)
        {
            if (string.IsNullOrWhiteSpace(updateContactDto.Name))
            {
                throw new ArgumentException("Contact name cannot be empty");
            }

            if (updateContactDto.Name.All(char.IsDigit))
            {
                throw new ArgumentException("Company name cannot contain only numbers");
            }

            Contact existingContact = await _contactRepository.GetById(updateContactDto.Id);

            if (existingContact == null)
            {
                _logger.LogError("Update failed: A contact was not found");
                throw new NotFoundException("No contact found to update");
            }

            existingContact.Name = updateContactDto.Name;

            await _contactRepository.Update(existingContact);
            return existingContact.Id;
        }

        public async Task DeleteContact(int id)
        {
            Contact contact = await _contactRepository.GetById(id);

            if (contact == null)
            {
                _logger.LogError($"Delete failed: Contact with id: {id} doesnt exists");
                throw new NotFoundException($"Contact with ID:{id} was not found");
            }

            _contactRepository.Delete(contact);
        }

        public async Task<List<ContactDto>> GetAll()
        {
            IReadOnlyList<Contact> contacts = await _contactRepository.Get();
            return _mapper.Map<List<ContactDto>>(contacts);
        }

        public async Task<List<ContactDto>> GetContactsWithCompanyAndCountry()
        {
            List<Contact> contacts = await _contactRepository.GetContactsWithCompanyAndCountry();
            return _mapper.Map<List<ContactDto>>(contacts);
        }

        public async Task<List<ContactDto>> FilterContacts(int countryId, int companyId)
        {
            IReadOnlyList<Contact> contacts = await _contactRepository.FilterContacts(countryId, companyId);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
    }
}
