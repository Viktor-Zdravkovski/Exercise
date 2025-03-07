using System.ComponentModel.DataAnnotations;

namespace BasicWeb.Domain
{
    public class User : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
