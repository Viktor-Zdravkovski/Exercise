using System.ComponentModel.DataAnnotations;

namespace BasicWeb.Dto.UserDto
{
    public class LogInUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
