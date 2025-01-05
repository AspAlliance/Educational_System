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
    public class SubLessonRepository : GenericReposiory<SubLessons>, ISubLessonRepository
    {
        private readonly Education_System _dbContext;
        public SubLessonRepository(Education_System dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSubLessonToCourseAsync(SubLessons subLessons, int courseId)
        {
            subLessons.CourseID = courseId;

            await _dbContext.SubLessons.AddAsync(subLessons);
            await _dbContext.SaveChangesAsync();
        }
    }
}
