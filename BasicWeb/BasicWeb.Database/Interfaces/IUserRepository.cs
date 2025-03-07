using BasicWeb.Domain;

namespace BasicWeb.Database.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> LogInUser(string email, string hashPassword);

        Task<User> GetUserByEmail(string email);
    }
}
