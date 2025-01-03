using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.DAL.Models;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ILessonRepository:IGenericRepository<Lessons>
    {
        Task<List<Lessons>> GetLessonsByCrsIdAsync(int crsId);
        Task<List<Comments>> GetAllCommentsByLessonId(int lessonId);
    }
}
