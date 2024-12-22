using EducationalSystem.BLL.Specification;
using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Courses>
    {
        Task<IQueryable<Courses>> GetCoursesByInstructorWithSpecsAsync(int instructorId, ISpecification<Courses> specification = null);

    }
}
