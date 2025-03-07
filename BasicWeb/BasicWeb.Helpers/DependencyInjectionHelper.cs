using BasicWeb.Database.Implementations;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Services.Implementations;
using BasicWeb.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BasicWeb.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectRepositories(this IServiceCollection services)
        {

            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IRepository<Company>, CompanyRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddLogging();
        }
    }
}
