using BasicWeb.Dto.UserDto;

namespace BasicWeb.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);

        Task<LogInResponseDto> LogInUser(LogInUserDto logInUserDto);
    }
}
