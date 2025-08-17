using EducationalSystem.BLL.Repositories.Interfaces;
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
    public class EnrollRepository : IEnrollRepository
    {
        private protected readonly Education_System _dbContext;
        public EnrollRepository(Education_System dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<bool> EnrollInCourseAsync(int courseId, string userId)
        {
            var alreadyEnrolled = await _dbContext.Set<Course_Enrollments>()
                .AnyAsync(e => e.CourseId == courseId && e.UserID == userId);

            if (alreadyEnrolled)
                return false;

            var enrollment = new Course_Enrollments
            {
                CourseId = courseId,
                UserID = userId
            };

            _dbContext.Set<Course_Enrollments>().Add(enrollment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IQueryable<ApplicationUser>> GetEnrolledUsersAsync(int courseId)
        {
            var users = _dbContext.Set<Course_Enrollments>()
                .Where(e => e.CourseId == courseId)
                .Select(e => e.User);

            return await Task.FromResult(users);
        }

        public async Task<int> GetEnrollmentCountAsync(int courseId)
        {
            var count = await _dbContext.Set<Course_Enrollments>()
                .CountAsync(e => e.CourseId == courseId);

            return count;
        }

        public async Task<IQueryable<Courses>> GetUserEnrolledCoursesAsync(string userId)
        {
            var courses = _dbContext.Set<Course_Enrollments>()
                .Where(e => e.UserID == userId)
                .Select(e => e.Courses);

            return await Task.FromResult(courses);
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(int courseId, string userId)
        {
            var enrolled = await _dbContext.Set<Course_Enrollments>()
                .AnyAsync(e => e.CourseId == courseId && e.UserID == userId);

            return enrolled;
        }

        public async Task<bool> UnenrollFromCourseAsync(int courseId, string userId)
        {
            var enrollment = await _dbContext.Set<Course_Enrollments>()
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserID == userId);

            if (enrollment == null)
                return false;

            _dbContext.Set<Course_Enrollments>().Remove(enrollment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
