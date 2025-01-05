using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Specification;
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
    public class CourseRepository : GenericReposiory<Courses>, ICourseRepository
    {
        private protected readonly Education_System _dbContext;
        public CourseRepository(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IQueryable<Courses>> GetCoursesByInstructorWithSpecsAsync(int instructorId, ISpecification<Courses> specification)
        {
            IQueryable<Courses> query = _dbContext.Set<Courses>()
                .Where(c => c.Course_Instructors.InstructorID == instructorId)
                .AsNoTracking(); 


            if (specification != null)
            {
                query = specification.Apply(query);
            }

            return await Task.FromResult(query);
        }
    }
}
