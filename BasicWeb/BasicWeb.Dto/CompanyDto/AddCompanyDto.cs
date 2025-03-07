using System.ComponentModel.DataAnnotations;

namespace BasicWeb.Dto.CompanyDto
{
    public class AddCompanyDto
    {
        [Required]
        public string Name { get; set; }
    }
}
