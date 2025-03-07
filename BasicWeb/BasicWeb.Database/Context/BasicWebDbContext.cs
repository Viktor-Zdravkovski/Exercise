using BasicWeb.Database.Extensions;
using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace BasicWeb.Database.Context
{
    public class BasicWebDbContext : DbContext
    {
        public BasicWebDbContext(DbContextOptions<BasicWebDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureCompanyEntity();

            modelBuilder.ConfigureContactEntity();

            modelBuilder.ConfigureCountryEntity();

            modelBuilder.ConfigureUsersEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
