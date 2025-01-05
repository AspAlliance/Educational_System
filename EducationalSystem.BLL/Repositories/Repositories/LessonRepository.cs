using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class LessonRepository : GenericReposiory<Lessons>, ILessonRepository
    {
        private readonly Education_System _dbContext;

        public LessonRepository(Education_System dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all lessons with optional filters
        public async Task<IEnumerable<Lessons>> GetLessonsAsync(int? sublessonId = null, string title = null)
        {
            var query = _dbContext.Lessons.AsQueryable();

            // Apply filters
            if (sublessonId.HasValue)
                query = query.Where(l => l.SubLessonID == sublessonId);

            if (!string.IsNullOrEmpty(title))
                query = query.Where(l => l.LessonTitle.Contains(title));

            return await query.ToListAsync();
        }

        // Get lessons by sublesson ID
        public async Task<IEnumerable<Lessons>> GetLessonsBySublessonAsync(int sublessonId)
        {
            return await _dbContext.Lessons
                .Where(l => l.SubLessonID == sublessonId)
                .ToListAsync();
        }

        // Get a specific lesson with prerequisites
        public async Task<Lessons> GetLessonWithPrerequisitesAsync(int lessonId)
        {
            return await _dbContext.Lessons
                .Include(l => l.PrerequisiteLessonPrerequisites)
                .FirstOrDefaultAsync(l => l.ID == lessonId);
        }

        // Get prerequisites for a specific lesson
        public async Task<IEnumerable<Lesson_Prerequisites>> GetPrerequisitesForLessonAsync(int lessonId)
        {
            return await _dbContext.Lesson_Prerequisites
                .Where(lp => lp.CurrentLessonID == lessonId)
                .ToListAsync();
        }

        // Add a prerequisite to a lesson
        public async Task<bool> AddPrerequisiteAsync(int lessonId, int prerequisiteLessonId)
        {
            var prerequisite = new Lesson_Prerequisites
            {
                CurrentLessonID = lessonId,
                PrerequisiteLessonID = prerequisiteLessonId
            };

            _dbContext.Lesson_Prerequisites.Add(prerequisite);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Mark a lesson as completed for a user
        public async Task<bool> MarkLessonAsCompletedAsync(int lessonId, string userId)
        {
            var completion = new Lesson_Completions
            {
                UserID = userId,
                LessonID = lessonId,
                CompletionDate = DateTime.UtcNow
            };

            _dbContext.Lesson_Completions.Add(completion);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Check if a user has completed all prerequisites for a lesson
        public async Task<bool> CheckPrerequisiteCompletionAsync(int lessonId, string userId)
        {
            var prerequisites = await _dbContext.Lesson_Prerequisites
                .Where(lp => lp.CurrentLessonID == lessonId)
                .Select(lp => lp.PrerequisiteLessonID)
                .ToListAsync();

            var completedLessons = await _dbContext.Lesson_Completions
                .Where(lc => lc.UserID == userId && prerequisites.Contains(lc.LessonID))
                .Select(lc => lc.LessonID)
                .ToListAsync();

            return prerequisites.All(p => completedLessons.Contains(p));
        }

        // Get lessons for a sublesson, ordered by prerequisite completion
        public async Task<IEnumerable<Lessons>> GetLessonsOrderedByPrerequisiteAsync(int sublessonId, string userId)
        {
            var lessons = await _dbContext.Lessons
                .Where(l => l.SubLessonID == sublessonId)
                .ToListAsync();

            var orderedLessons = new List<Lessons>();
            foreach (var lesson in lessons)
            {
                if (await CheckPrerequisiteCompletionAsync(lesson.ID, userId))
                {
                    orderedLessons.Add(lesson);
                }
            }

            return orderedLessons;
        }
        public async Task AddLessonToCourseAsync(Lessons lesson, int courseId)
        {
            // Associate the lesson with the course
            lesson.CourseID = courseId;

            await _dbContext.Lessons.AddAsync(lesson);
            await _dbContext.SaveChangesAsync();
        }

        // Delete a lesson by ID
        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _dbContext.Lessons.Remove(lesson);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task AddSubLessonToCourseAsync(SubLessons subLessons , int courseId)
        {
            subLessons.CourseID = courseId;

            await _dbContext.SubLessons.AddAsync(subLessons);
            await _dbContext.SaveChangesAsync();
        }
    }
}