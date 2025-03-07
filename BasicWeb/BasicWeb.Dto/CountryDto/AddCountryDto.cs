using System.ComponentModel.DataAnnotations;

namespace BasicWeb.Dto.CountryDto
{
    public class AddCountryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
