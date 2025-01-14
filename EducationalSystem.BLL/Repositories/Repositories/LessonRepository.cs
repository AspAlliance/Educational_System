using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class LessonRepository : GenericReposiory<Lessons>, ILessonRepository
    {
        private readonly Education_System _dbContext;
        public LessonRepository(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<Lesson_Completions> existingCompletion(string userId, int lessonId)
        {
            var existingCompletion = await _dbContext.Lesson_Completions
            .FirstOrDefaultAsync(lc => lc.UserID == userId && lc.LessonID == lessonId);
            return existingCompletion;
        }



        public async Task<List<Comments>> GetAllCommentsByLessonId(int lessonId)
        {
            var comments = await _dbContext.Comments
                .Where(li => li.LessonID == lessonId)
                .ToListAsync();

            return comments;
        }

        public async Task<List<Lessons>> GetLessonsOrderedByPrerequisiteCompletion(int subLessonId, string userId)
        {
            // Fetch lessons for the specific subLessonId
            var lessons = await _dbContext.Lessons
                .Where(l => l.SubLessonID == subLessonId)
                .ToListAsync();

            var completedLessons = await _dbContext.Lesson_Completions
                .Where(lc => lc.UserID == userId && lc.Lessons.SubLessonID == subLessonId)
                .Select(lc => lc.Lessons) 
                .ToListAsync();

            //// Order lessons by the count of prerequisites completed
            //var orderLessons = lessons
            //    .OrderBy(l => l.PrerequisiteLessonPrerequisites.Count == 0 ? 0 : l.PrerequisiteLessonPrerequisites.Count(p => completedLessonsIds.Contains(p.PrerequisiteLessonID)))
            //    .ThenBy(l => l.ID)
            //    .ToList();

            return completedLessons;
        }

        public async Task<List<Lesson_Prerequisites>> GetLessonPrerequisitesByIdAsync(int lessonId)
        {
            var LessonPrerequisites = await _dbContext.Lesson_Prerequisites
                .Where(lp => lp.CurrentLessonID == lessonId)
                .ToListAsync();

            return LessonPrerequisites;
        }

        public async Task<List<Lessons>> GetLessonsByCrsIdAsync(int crsId)
        {
            var lessons = await _dbContext.Lessons
                .Where(l => l.CourseID == crsId)
                .ToListAsync();
           
            return lessons;
        }

        public Task<List<Lessons>> GetLessonsByIdsAsync(List<int> lessonsIds)
        {
            var lessons = _dbContext.Lessons.Where(l => lessonsIds.Contains(l.ID))
                .ToListAsync();
            return lessons;
        }

        public async Task<List<Lessons>> GetLessonsBySubLessonIdAsync(int subLessonId)
        {
            var lessons = await _dbContext.Lessons
                 .Where(l => l.SubLessonID == subLessonId)
                 .ToListAsync();

            return lessons;
        }

        

    }
}
