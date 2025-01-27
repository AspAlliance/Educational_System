using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class QuestionRepository : GenericReposiory<Questions>, IQuestionRepository
    {
        private readonly Education_System _dbContext;
        public QuestionRepository(Education_System dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<Questions>> GetAllByAssesmentId(int assesmentId)
        {
            var questions = await _dbContext.Questions.Where(q => q.AssessmentID == assesmentId).ToListAsync();
            return questions;
        }

        public async Task<Questions> GetQuestionWithAssesmentAndQuesType(int questionId)
        {
            var question = await _dbContext.Questions
                .Include(q => q.Assessments)
                .Include(q => q.QuestionType)
                .FirstOrDefaultAsync(q => q.ID == questionId);
            return question;
        }
    }
}
