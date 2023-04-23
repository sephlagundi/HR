using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRWeb.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {

            // Seed Roles

            List<IdentityRole> roles = new List<IdentityRole>()
    {
        new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
        new IdentityRole { Name = "User", NormalizedName = "USER" }
    };

            builder.Entity<IdentityRole>().HasData(roles);

            // -----------------------------------------------------------------------------

            // Seed Users

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            List<ApplicationUser> users = new List<ApplicationUser>()
    {
         // imporant: don't forget NormalizedUserName, NormalizedEmail 
                 new ApplicationUser {
                     FirstName = "Admin",
                     LastName = "Admin",
                    UserName = "admin@admin.com",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    DOB = new DateTime(0001, 01, 01),
                    PhoneNumber = "000000000",


    },

/*                new ApplicationUser {
                    FirstName = "Admin2",
                    LastName = "lastname",
                    UserName = "user3@hotmail.com",
                    NormalizedUserName = "USER3@HOTMAIL.COM",
                    Email = "user3@hotmail.com",
                    NormalizedEmail = "USER3@HOTMAIL.COM",
                    EmailConfirmed = true,
                },*/
    };


            builder.Entity<ApplicationUser>().HasData(users);

            ///----------------------------------------------------

            // Seed UserRoles


            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            // Add Password For All Users

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "admin");
            /*users[1].PasswordHash = passwordHasher.HashPassword(users[1],*//* "User.155");*/

            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId =
            roles.First(q => q.Name == "Administrator").Id
            });

            /*            userRoles.Add(new IdentityUserRole<string>
                        {
                            UserId = users[1].Id,
                            RoleId =
                        roles.First(q => q.Name == "Admin").Id
                        });*/


            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

        }
    }
}