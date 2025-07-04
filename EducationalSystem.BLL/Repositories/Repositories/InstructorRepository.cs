﻿using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class InstructorRepository : GenericReposiory<Instructors>, IInstructorRepository
    {
        private protected readonly Education_System _dbContext;
        public InstructorRepository(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IEnumerable<ApplicationUser>> GeInstructorUsersAsync(int instructorId)
        {
            var users = await _dbContext.Users
                .Where(user => user.User_Instructor.Any(ui => ui.InstructorId == instructorId))
                .AsNoTracking()
                .ToListAsync();

            return users.AsEnumerable(); // Convert List to IEnumerable
        }

        public async Task<ApplicationUser?> GetInstructorUserByIdAsync(int instructorId)
        {
            return await _dbContext.Set<Instructors>()
           .Where(i => i.ID == instructorId)
           .Select(i => i.applicationUser)
           .FirstOrDefaultAsync();
        }

        public async Task<Specializations> GetInstructorsBySpecializationIdAsync(int? instructorId)
        {
            return await _dbContext.Set<Instructors>()
                .Where(i => i.ID == instructorId)
                .Select(i => i.Specializations)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Instructors>> GetInstructorsByCrsID(int crsId)
        {
            var instructor = await _dbContext.Course_Instructors
                .Where(ci => ci.ID == crsId)
                .Select(c => c.Instructors)
                .ToListAsync();

            return instructor;
        }

        public async Task<Courses> GetCrsByIdAsync(int crsId)
        {
            var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.ID == crsId);

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with id {crsId} not found");
            }

            return course;
        }
    }
}
