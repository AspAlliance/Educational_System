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
            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Categories.json");
                var categories = JsonSerializer.Deserialize<List<Categories>>(categoriesData);

                foreach (var item in categories)
                {
                    await context.Set<Categories>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Courses.Any())
            {
                var coursesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Courses.json");
                var courses = JsonSerializer.Deserialize<List<Courses>>(coursesData);

                foreach (var item in courses)
                {
                    await context.Set<Courses>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.SubLessons.Any())
            {
                var subLessonsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/SubLessons.json");
                var subLessons = JsonSerializer.Deserialize<List<SubLessons>>(subLessonsData);

                foreach (var item in subLessons)
                {
                    await context.Set<SubLessons>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Lessons.Any())
            {
                var lessonsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Lessons.json");
                var lessons = JsonSerializer.Deserialize<List<Lessons>>(lessonsData);

                foreach (var item in lessons)
                {
                    await context.Set<Lessons>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Assessments.Any())
            {
                var assessmentsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Assessments.json");
                var assessments = JsonSerializer.Deserialize<List<Assessments>>(assessmentsData);

                foreach (var item in assessments)
                {
                    await context.Set<Assessments>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

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

            if (!context.questionTypes.Any())
            {
                var questionTypesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/QuestionTypes.json");
                var questionTypes = JsonSerializer.Deserialize<List<QuestionType>>(questionTypesData);

                foreach (var item in questionTypes)
                {
                    await context.Set<QuestionType>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Questions.Any())
            {
                var questionsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Questions.json");
                var questions = JsonSerializer.Deserialize<List<Questions>>(questionsData);

                foreach (var item in questions)
                {
                    await context.Set<Questions>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Choices.Any())
            {
                var choicesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Choices.json");
                var choices = JsonSerializer.Deserialize<List<Choices>>(choicesData);

                foreach (var item in choices)
                {
                    await context.Set<Choices>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Rubrics.Any())
            {
                var rubricsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Rubrics.json");
                var rubrics = JsonSerializer.Deserialize<List<Rubrics>>(rubricsData);

                foreach (var item in rubrics)
                {
                    await context.Set<Rubrics>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Discounts.Any())
            {
                var discountsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Discounts.json");
                var discounts = JsonSerializer.Deserialize<List<Discounts>>(discountsData);

                foreach (var item in discounts)
                {
                    await context.Set<Discounts>().AddAsync(item);
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

            if (!context.User_Instructor.Any())
            {
                var user_InstructorData = File.ReadAllText("../EducationalSystem.BLL/Seeding/User_Instructor.json");
                var user_Instructor = JsonSerializer.Deserialize<List<User_Instructor>>(user_InstructorData);

                foreach (var item in user_Instructor)
                {
                    await context.Set<User_Instructor>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Course_Enrollments.Any())
            {
                var course_EnrollmentsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Course_Enrollments.json");
                var course_Enrollments = JsonSerializer.Deserialize<List<Course_Enrollments>>(course_EnrollmentsData);

                foreach (var item in course_Enrollments)
                {
                    await context.Set<Course_Enrollments>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Course_Instructors.Any())
            {
                var course_InstructorsData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Course_Instructors.json");
                var course_Instructors = JsonSerializer.Deserialize<List<Course_Instructors>>(course_InstructorsData);

                foreach (var item in course_Instructors)
                {
                    await context.Set<Course_Instructors>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Lesson_Prerequisites.Any())
            {
                var lesson_PrerequisitesData = File.ReadAllText("../EducationalSystem.BLL/Seeding/Lesson_Prerequisites.json");
                var lesson_Prerequisites = JsonSerializer.Deserialize<List<Lesson_Prerequisites>>(lesson_PrerequisitesData);

                foreach (var item in lesson_Prerequisites)
                {
                    await context.Set<Lesson_Prerequisites>().AddAsync(item);
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
