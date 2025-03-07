using AutoMapper;
using BasicWeb.Domain;
using BasicWeb.Dto.CompanyDto;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Dto.CountryDto;

namespace BasicWeb.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<AddCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<AddContactDto, Contact>();
            CreateMap<UpdateContactDto, Contact>();

            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<AddCountryDto, Country>();
            CreateMap<UpdateCountryDto, Country>();
        }
    }
}
