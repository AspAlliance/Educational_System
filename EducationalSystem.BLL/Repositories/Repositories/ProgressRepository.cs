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

        public async Task<int> UpdateProgressAsync(string userId, int lessonId , int courseId)
        {


            if (courseId == 0)
                return 0; // مفيش كورس للدرس ده

            var lessonCompleted = await _dbContext.Lesson_Completions
                .Where(lc => lc.UserID == userId && lc.Lessons.CourseID == courseId)
                .CountAsync();

            var totalLessons = await _dbContext.Lessons
                .Where(l => l.CourseID == courseId)
                .CountAsync();

            if (totalLessons == 0)
                return 0;
            // 5/6 = 0.8333 , 4/6 = 0.6666 / 2/6 = 0.3333
            // احسب النسبة صح
            var score = (int)((lessonCompleted / (double)totalLessons) * 100);

            var progress = await _dbContext.progresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.CourseID == courseId);

            if (progress == null)
            {
                // لو مفيش Progress قبل كده، اعمله جديد
                progress = new Progress
                {
                    UserID = userId,
                    CourseID = courseId,
                    Score = score,
                    CompletedDate = DateTime.UtcNow
                };
                // if there is no progress record, add a new one
                // else update the existing one
                _dbContext.progresses.Add(progress);
            }
            else
            {
                progress.Score = score;
                progress.CompletedDate = DateTime.UtcNow;
                _dbContext.progresses.Update(progress);
            }

            await _dbContext.SaveChangesAsync();
            return score;
        }
    }
}
