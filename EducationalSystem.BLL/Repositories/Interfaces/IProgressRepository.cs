using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IProgressRepository
    {
        // get all progress records
        Task<IQueryable<Progress>> GetAllProgressAsync();
        // get a specific progress record by userId and courseId
        Task<Progress> GetProgressAsync(string userId, int courseId);
        // get the progress of a user in a specific course
        Task<Progress> GetProgressByUserIdAndCourseIdAsync(string userId, int courseId);
        // update the progress of a user in a specific course
        Task<int> UpdateProgressAsync(string userId, int lessonId);
        // add a new progress record for a user in a specific course
        Task<bool> AddProgressAsync(string userId, int courseId, int score);
        // get all progress records for a specific course
        Task<IQueryable<Progress>> GetProgressByCourseIdAsync(int courseId);
        // get all progress records for a specific user
        Task<IQueryable<Progress>> GetProgressByUserIdAsync(string userId);
        // delete a progress record by userId and courseId
        Task<bool> DeleteProgressAsync(string userId, int courseId);
        // check if a progress record exists for a user in a specific course
        Task<bool> ProgressExistsAsync(string userId, int courseId);
        // get the average score of all users in a specific course
        Task<double> GetAverageScoreByCourseIdAsync(int courseId);
        
    }
}
