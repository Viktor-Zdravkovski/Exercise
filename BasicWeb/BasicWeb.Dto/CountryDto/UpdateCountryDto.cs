using System.ComponentModel.DataAnnotations;

namespace BasicWeb.Dto.CountryDto
{
    public class UpdateCountryDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
