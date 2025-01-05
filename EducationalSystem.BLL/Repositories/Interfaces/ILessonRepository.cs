using EducationalSystem.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lessons>
    {
        // Get all lessons with optional filters
        Task<IEnumerable<Lessons>> GetLessonsAsync(int? sublessonId = null, string title = null);

        // Get lessons by sublesson ID
        Task<IEnumerable<Lessons>> GetLessonsBySublessonAsync(int sublessonId);

        // Get a specific lesson with prerequisites
        Task<Lessons> GetLessonWithPrerequisitesAsync(int lessonId);

        // Get prerequisites for a specific lesson
        Task<IEnumerable<Lesson_Prerequisites>> GetPrerequisitesForLessonAsync(int lessonId);

        // Add a prerequisite to a lesson
        Task<bool> AddPrerequisiteAsync(int lessonId, int prerequisiteLessonId);

        // Mark a lesson as completed for a user
        Task<bool> MarkLessonAsCompletedAsync(int lessonId, string userId);

        // Check if a user has completed all prerequisites for a lesson
        Task<bool> CheckPrerequisiteCompletionAsync(int lessonId, string userId);

        // Get lessons for a sublesson, ordered by prerequisite completion
        Task<IEnumerable<Lessons>> GetLessonsOrderedByPrerequisiteAsync(int sublessonId, string userId);
        // Add a lesson to a course
        Task AddLessonToCourseAsync(Lessons lesson, int courseId);

        // Add a AddSubLesson to a course
        Task AddSubLessonToCourseAsync(SubLessons subLessons, int courseId);


        // Delete a lesson by ID
        Task DeleteLessonAsync(int id);
    }
}