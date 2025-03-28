﻿using AutoMapper;
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
    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> _logger;
        private readonly IRepository<Company> _companyRepo;
        private readonly IMapper _mapper;

        public CompanyService(ILogger<CompanyService> logger, IRepository<Company> companyRepo, IMapper mapper)
        {
            _logger = logger;
            _companyRepo = companyRepo;
            _mapper = mapper;
        }

        public async Task<int> AddCompany(AddCompanyDto addCompanyDto)
        {
            if (string.IsNullOrWhiteSpace(addCompanyDto.Name))
            {
                throw new ArgumentException("Company name cannot be empty.");
            }

            if (addCompanyDto.Name.All(char.IsDigit))
            {
                throw new ArgumentException("Company name cannot contain only numbers.");
            }

            Company company = _mapper.Map<Company>(addCompanyDto);
            await _companyRepo.Create(company);
            return company.Id;
        }

        public async Task<int> UpdateCompany(UpdateCompanyDto updateCompanyDto)
        {
            if (string.IsNullOrWhiteSpace(updateCompanyDto.Name))
            {
                throw new ArgumentException("Company name cannot be empty");
            }

            Company existingCompany = await _companyRepo.GetById(updateCompanyDto.Id);

            if (existingCompany == null)
            {
                _logger.LogError("Update Failed: A company was not found");
                throw new NotFoundException("No company found to update");
            }

            int id = existingCompany.UpdateName(updateCompanyDto.Name);

            await _companyRepo.Update(existingCompany);
            return id;
        }

        public async Task DeleteCompany(int id)
        {
            Company company = await _companyRepo.GetById(id);

            if (company == null)
            {
                _logger.LogError($"Delete failed: Company with id: {id} doesnt exists");
                throw new NotFoundException($"Company with ID: {id} was not found");
            }


            _companyRepo.Delete(company);
        }

        public async Task<List<CompanyDto>> GetAll()
        {
            IReadOnlyList<Company> companies = await _companyRepo.Get();

            if (!companies.Any())
            {
                _logger.LogError("No companies found");
                throw new NotFoundException("No companies found");
            }

            return _mapper.Map<List<CompanyDto>>(companies);
        }
    }
}
