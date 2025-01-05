using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ISubLessonRepository: IGenericRepository<SubLessons>
    {
        Task AddSubLessonToCourseAsync(SubLessons subLessons, int courseId);
    }
}
