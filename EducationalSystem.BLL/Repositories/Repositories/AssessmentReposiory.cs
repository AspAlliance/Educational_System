using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class AssessmentReposiory : GenericReposiory<Assessments>, IAssessmentRepository
    {
        private protected readonly Education_System _dbContext;

        public AssessmentReposiory(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<List<Assessments>> GetAllByCrsId(int crsId)
        {
           var assesments = await _dbContext.Assessments.Where(A => A.CourseID == crsId).ToListAsync();
            return assesments;
        }

        public async Task<List<Assessments>> GetAllByLessonId(int lessonId)
        {
            var assesmnts = await _dbContext.Assessments.Where(A => A.LessonID == lessonId).ToListAsync();
            return assesmnts;
        }

        public async Task<List<Course_Enrollments>> GetCoursesEnrollmentByUserId(string studentId)
        {
            var courseEnrollment = await _dbContext.Course_Enrollments.Where
                (c => c.UserID == studentId).Include(c => c.Courses).ToListAsync();

            return courseEnrollment;
        }

        public async Task<List<Assessment_Results>> GetResultsByStudentIdAsync(string studentId)
        {
            var results = await _dbContext.Assessment_Results
                .Where(ar => ar.UserID == studentId)
                .Include(ar => ar.User) // Includes student details
                .ToListAsync();

            return results;
        }



    }
}
