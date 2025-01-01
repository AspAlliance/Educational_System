using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IInstructorRepository : IGenericRepository<Instructors>
    {
        Task<IEnumerable<ApplicationUser>> GeInstructorUsersAsync(int instructorId);
        //Task<InstructorWUserWSpecial?> GetSpecializationByInstructorIdAsync(int instructorId);
        Task<ApplicationUser?> GetInstructorUserByIdAsync(int instructorId);
        Task<Specializations> GetInstructorsBySpecializationIdAsync(int? instructorId);
        Task<List<Instructors>> GetInstructorsByCrsID(int crsId);
        Task<Courses> GetCrsByIdAsync(int crsId);
        
    }
}
