using BasicWeb.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BasicWeb.Database.Extensions
{
    public static class EFExtensions
    {
        public static void ConfigureCountryEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.HasKey(x => x.Id).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                //entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name)
                      //.HasColumnType("nvarchar(30)")
                      .IsRequired();

                // added on delete
                entity.HasMany(x => x.Contacts)
                      .WithOne()
                      .HasForeignKey(x => x.CountryId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureCompanyEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasKey(x => x.Id).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                //entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name)
                      //.HasColumnType("nvarchar(50)")
                      .IsRequired();

                // Added
                entity.HasMany(x => x.Contacts)
                      .WithOne()
                      .HasForeignKey(x => x.CompanyId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public static void ConfigureContactEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.HasKey(x => x.Id).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                //entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name)
                      //.HasColumnType("nvarchar(50)")
                      .IsRequired();

                entity.Property(x => x.CompanyId)
                      //.HasColumnType("bigint")
                      .IsRequired();

                entity.Property(x => x.CountryId)
                      //.HasColumnType("bigint")
                      .IsRequired();


                // added on delete to both
                entity.HasOne(x => x.Company)
                      .WithMany()
                      .HasForeignKey(x => x.CompanyId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Country)
                      .WithMany()
                      .HasForeignKey(x => x.CountryId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        // =========================================
        public static void ConfigureUsersEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(x => x.Id).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                entity.Property(x => x.Email)
                      .IsRequired();

                entity.Property(x => x.Password)
                      .IsRequired();
            });
        }
    }
}
