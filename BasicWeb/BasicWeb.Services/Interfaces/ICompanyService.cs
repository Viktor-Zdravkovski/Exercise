using BasicWeb.Dto.CompanyDto;

namespace BasicWeb.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<int> AddCompany(AddCompanyDto addCompanyDto);

        Task<int> UpdateCompany(UpdateCompanyDto updateCompanyDto);

        void DeleteCompany(int id);

        Task<List<CompanyDto>> GetAll();
    }
}
