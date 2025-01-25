using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.DAL.Models;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IAssessmentRepository:IGenericRepository<Assessments>
    {
        Task<List<Assessments>> GetAllByCrsId(int crsId);
        Task<List<Assessments>> GetAllByLessonId(int lessonId);
        Task<List<Assessment_Results>> GetResultsByStudentIdAsync(string studentId);
        Task<List<Course_Enrollments>> GetCoursesEnrollmentByUserId(string studentId);
    }
}
