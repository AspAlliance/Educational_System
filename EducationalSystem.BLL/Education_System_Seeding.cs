using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EducationalSystem.BLL
{
    public static class Education_System_Seeding
    {
        public async static Task SeedAsync(Education_System context)
        {

            if (!context.Specializations.Any())
            {
                var specializationsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Specializations.json");
                var specializations = JsonSerializer.Deserialize<List<Specializations>>(specializationsData);

                foreach (var item in specializations)
                {
                    await context.Set<Specializations>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }
            if (!context.Instructors.Any())
            {
            
                var instructorsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Instructors.json");
                var instructors = JsonSerializer.Deserialize<List<Instructors>>(instructorsData);

                foreach (var item in instructors)
                {
                    await context.Set<Instructors>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }   

        }

        public async static Task SeedUsersAndRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            #region RoleSeeding
            // Seed Roles
            //if (!roleManager.Roles.Any())
            //{
            //    var rolesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Roles.json");
            //    var roles = JsonSerializer.Deserialize<List<string>>(rolesData); // JSON should contain a list of role names

            //    foreach (var roleName in roles)
            //    {
            //        if (!await roleManager.RoleExistsAsync(roleName))
            //        {
            //            await roleManager.CreateAsync(new IdentityRole(roleName));
            //        }
            //    }
            //} 
            #endregion

            // Seed Users
            if (!userManager.Users.Any())
            {
                var usersData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Users.json");
                var users = JsonSerializer.Deserialize<List<ApplicationUser>>(usersData);

                foreach (var user in users)
                {
                    //var existingUser = await userManager.FindByEmailAsync(user.Email);
                    //if (existingUser == null)
                    //{

                    //    // Create the user
                    //    var result = await userManager.CreateAsync(user, "Default@Password1"); // Replace with a default password

                    //}

                    var applicationUser = new ApplicationUser
                    {
                        // Use the ID directly from JSON
                        Id = user.Id, // Note the capitalization
                        UserName = user.UserName,
                        NormalizedUserName = user.NormalizedUserName ?? user.UserName.ToUpper(),
                        Email = user.Email,
                        NormalizedEmail = user.NormalizedEmail ?? user.Email.ToUpper(),
                        EmailConfirmed = user.EmailConfirmed,
                        PasswordHash = user.PasswordHash,
                        SecurityStamp = user.SecurityStamp,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        LockoutEnd = user.LockoutEnd,
                        LockoutEnabled = user.LockoutEnabled,
                        AccessFailedCount = user.AccessFailedCount,

                        // Additional custom fields
                        Name = user.Name,
                        ProfileImageURL = user.ProfileImageURL
                    };

                    // Create user directly with existing password hash
                    var result = await userManager.CreateAsync(applicationUser);
                }
            }
    }

    }
}
