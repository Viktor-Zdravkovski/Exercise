using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.UserDto;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.Configuration;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace BasicWeb.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepo;
        private readonly BasicWebAppSettings _appSettings;

        public UserService(ILogger<UserService> logger, IUserRepository userRepo, IOptions<BasicWebAppSettings> appSettings)
        {
            _logger = logger;
            _userRepo = userRepo;
            _appSettings = appSettings.Value;
        }

        public async Task<LogInResponseDto> LogInUser(LogInUserDto logInUserDto)
        {

            if (string.IsNullOrEmpty(logInUserDto.Email) || string.IsNullOrEmpty(logInUserDto.Password))
            {
                _logger.LogError("Login attempt failed for email: {Email}. Missing data.", logInUserDto.Email);
                throw new NoDataException("Users data is required");
            }

            string hashPassword = HashPassword(logInUserDto.Password);
            User userDb = await _userRepo.LogInUser(logInUserDto.Email, hashPassword);

            if (userDb == null)
            {
                _logger.LogError("Login attempt failed. User not found");
                throw new NotFoundException("User was not found!");
            }

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("UserId", userDb.Id.ToString()),
                        new Claim(ClaimTypes.Email, userDb.Email),
                    }
                )
            };

            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new LogInResponseDto
            {
                Token = token,
            };
        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            await ValidateUser(registerUserDto);

            var hashPassword = HashPassword(registerUserDto.Password);

            User user = new User
            {
                Name = registerUserDto.Name,
                Email = registerUserDto.Email,
                Password = hashPassword
            };

            await _userRepo.Create(user);
        }

        private string HashPassword(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

            byte[] hashBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            string hashPassword = Encoding.ASCII.GetString(hashBytes);

            return hashPassword;
        }

        private async Task ValidateUser(RegisterUserDto registerUserDto)
        {
            if ((string.IsNullOrEmpty(registerUserDto.Email)) || (string.IsNullOrEmpty(registerUserDto.Password)))
            {
                _logger.LogError("Register attempt failed. Missing Email or Password.");
                throw new NoDataException("Email & Password are required fields!");
            }

            if (registerUserDto.Email.Length > 50)
            {
                _logger.LogError("Register attempt failed. Email exceeds 50 characters.");
                throw new NoDataException("Username: Maximum length of the username is longer than 50 characters");
            }

            if (string.IsNullOrEmpty(registerUserDto.Name))
            {
                _logger.LogError("Register attempt failed. Name is missing.");
                throw new NoDataException("Name is required field");
            }

            var userDb = await _userRepo.GetUserByEmail(registerUserDto.Email);
            if (userDb != null)
            {
                _logger.LogError("Register attempt failed. Email already exists.");
                throw new InvalidOperationException($"The user with username: {registerUserDto.Email} already exists");
            }
        }
    }
}
