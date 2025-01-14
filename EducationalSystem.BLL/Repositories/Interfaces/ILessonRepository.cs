using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ILessonRepository:IGenericRepository<Lessons>
    {
        Task<List<Lessons>> GetLessonsByCrsIdAsync(int crsId);
        Task<List<Comments>> GetAllCommentsByLessonId(int lessonId);
        Task <List<Lessons>> GetLessonsBySubLessonIdAsync(int subLessonId);
        Task<List<Lesson_Prerequisites>> GetLessonPrerequisitesByIdAsync(int lessonId);
        Task<List<Lessons>> GetLessonsByIdsAsync (List<int> lessonsIds);
        Task<Lesson_Completions> existingCompletion(string userId, int lessonId);
        Task<List<Lessons>> GetLessonsOrderedByPrerequisiteCompletion(int subLessonId, string userId);

    }
}
