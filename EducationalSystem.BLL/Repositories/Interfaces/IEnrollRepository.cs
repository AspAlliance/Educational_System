using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IEnrollRepository
    {
        Task<bool> EnrollInCourseAsync(int courseId, string userId);
        Task<bool> UnenrollFromCourseAsync(int courseId, string userId);
        Task<bool> IsUserEnrolledInCourseAsync(int courseId, string userId);
        Task<int> GetEnrollmentCountAsync(int courseId);
        Task<IQueryable<ApplicationUser>> GetEnrolledUsersAsync(int courseId);
        Task<IQueryable<Courses>> GetUserEnrolledCoursesAsync(string userId);
    }
}
