using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
            //Accepts DbContextOptions for configuration(connection strings, database provider, etc.)
        }

        //method overriding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); //ensures Identity’s default table configurations are preserved.

            // when migrations run EF core will  create and insert
            //3 roles into AspNetRoles table
            //1 user into AspNetUsers table
            //3 user - role relationships into AspNetUserRoles 

            //These are the Guids created from c# interactive window by command : Console.WriteLine(Guid.NewGuid());
            var adminRoleId = "f09d248f-0124-4cf4-b5b4-9de528ca9df5";
            var superAdminRoleId = "a791c349-c7aa-4d73-a79e-6a3a6fcdbb1c";
            var userRoleId = "f203909c-651c-45c5-a4e4-d39e9784b0bb";

            //Create Roles(User,Admin,SuperAdmin)
            var roles = new List<IdentityRole>
            {
               new IdentityRole
               {
                   Name="Admin",
                   NormalizedName="Admin",
                   Id = adminRoleId,
                   ConcurrencyStamp = adminRoleId
               },
               new IdentityRole
               {
                   Name="SuperAdmin",
                   NormalizedName="SuperAdmin",
                   Id = superAdminRoleId,
                   ConcurrencyStamp = superAdminRoleId
               },
               new IdentityRole
               {
                   Name="User",
                   NormalizedName="User",
                   Id = userRoleId,
                   ConcurrencyStamp = userRoleId
               }
            };

            builder.Entity<IdentityRole>().HasData(roles);  //Insert roles in builder object for migrations 

            //Create SuperAdminUser

            var superAdminId = "0e36e06d-0c6f-4ccf-b635-3ffc2574ba5b";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId,
                // Use a fixed, pre-generated hash instead of regenerating it
                PasswordHash = "AQAAAAIAAYagAAAAEEiDmtkhaAgnmZhmswODWlY2wIKxHShqIR5cxUXia2ZfPtw0N7eBgkE+DJgGb1qsHw==",
                SecurityStamp = "f92a7c13-09bb-43c0-9303-8dfd9ab3d27f",
                ConcurrencyStamp = "8e4d3d59-5d6b-4adf-98cf-bc1b7a1a63de"
            };

            //Uses ASP.NET Core Identity’s PasswordHasher to hash the password securely
            //superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add all roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                RoleId = adminRoleId,
                UserId= superAdminId
                },
                new IdentityUserRole<string>
                {
                RoleId = superAdminRoleId,
                UserId= superAdminId
                },
                new IdentityUserRole<string>
                {
                RoleId = userRoleId,
                UserId= superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);



        }
    }
}
