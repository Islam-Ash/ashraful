using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            var roleAdmin = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString() // Optional
            };
            modelBuilder.Entity<ApplicationRole>().HasData(roleAdmin);

            // Seed Admin User
            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "aislam.cse.1023@gmail.com",
                NormalizedUserName = "AISLAM.CSE.1023@GMAIL.COM",
                Email = "aislam.cse.1023@gmail.com",
                NormalizedEmail = "AISLAM.CSE.1023@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "01518745450",
                FirstName = "Ashraful",
                LastName = "Islam",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Hash the password
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123456");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Seed User-Role relationship
            modelBuilder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
            {
                UserId = adminUser.Id,
                RoleId = roleAdmin.Id
            });
        }

    }
}
