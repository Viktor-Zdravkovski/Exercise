﻿using BasicWeb.Domain;
using BasicWeb.Dto.ContactsDto;

namespace BasicWeb.Services.Interfaces
{
    public interface IContactService
    {
        Task<int> AddContact(AddContactDto addContactDto);

        Task<int> UpdateContact(UpdateContactDto updateContactDto);

        void DeleteContact(int id);

        Task<List<ContactDto>> GetAll();

        Task<List<ContactDto>> GetContactsWithCompanyAndCountry();

        Task<List<ContactDto>> FilterContacts(int countryId, int companyId);


        // smeni int mesto void VO SITE
    }
}
