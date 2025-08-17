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
    public class ProgressRepository : IProgressRepository
    {
        private protected readonly Education_System _dbContext;
        public ProgressRepository(Education_System dbcontext)
        {
            _dbContext = dbcontext;
        }

        public Task<bool> AddProgressAsync(string userId, int courseId, int score)
        {
            var progress = new Progress
            {
                UserID = userId,
                CourseID = courseId,
                Score = score,
                CompletedDate = DateTime.UtcNow
            };
            _dbContext.progresses.Add(progress);
            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public Task<bool> DeleteProgressAsync(string userId, int courseId)
        {
            var progress = _dbContext.progresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.CourseID == courseId);
            if (progress == null)
            {
                return Task.FromResult(false);
            }
            _dbContext.progresses.Remove(progress.Result);
            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public Task<IQueryable<Progress>> GetAllProgressAsync()
        {
            return Task.FromResult(_dbContext.progresses.AsQueryable());
        }

        public Task<double> GetAverageScoreByCourseIdAsync(int courseId)
        {
            return _dbContext.progresses
                .Where(p => p.CourseID == courseId)
                .AverageAsync(p => p.Score);
        }

        public Task<Progress> GetProgressAsync(string userId, int courseId)
        {
            return _dbContext.progresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.CourseID == courseId);
        }

        public Task<IQueryable<Progress>> GetProgressByCourseIdAsync(int courseId)
        {
            return Task.FromResult(_dbContext.progresses
                .Where(p => p.CourseID == courseId)
                .AsQueryable());
        }

        public Task<Progress> GetProgressByUserIdAndCourseIdAsync(string userId, int courseId)
        {
            return _dbContext.progresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.CourseID == courseId);
        }

        public Task<IQueryable<Progress>> GetProgressByUserIdAsync(string userId)
        {
            return Task.FromResult(_dbContext.progresses
                .Where(p => p.UserID == userId)
                .AsQueryable());
        }

        public Task<bool> ProgressExistsAsync(string userId, int courseId)
        {
            return _dbContext.progresses
                .AnyAsync(p => p.UserID == userId && p.CourseID == courseId);
        }

        public Task<int> UpdateProgressAsync(string userId, int lessonId)
        {
            var courseId = _dbContext.Lessons
                .Where(l => l.ID == lessonId)
                .Select(l => l.CourseID)
                .FirstOrDefault();

            var lessoncompleted = _dbContext.Lesson_Completions
            .Where(lc => lc.LessonID == lessonId && lc.UserID == userId).Count();
            var totalLessons = _dbContext.Lessons
                .Where(l => l.CourseID == courseId)
                .Count();
            var score = totalLessons / lessoncompleted * 100;
            var progress = _dbContext.progresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.CourseID == courseId);
            if (progress == null)
            {
                return Task.FromResult(0);
            }
            progress.Result.Score = score;
            progress.Result.CompletedDate = DateTime.UtcNow;
            _dbContext.progresses.Update(progress.Result);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(score > 0 ? score : 0);
        }
    }
}
