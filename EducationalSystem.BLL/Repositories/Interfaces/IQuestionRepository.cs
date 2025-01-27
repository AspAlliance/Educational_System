using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.DAL.Models;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Questions>
    {
        Task<List<Questions>> GetAllByAssesmentId(int assesmentId);
        Task<Questions> GetQuestionWithAssesmentAndQuesType(int questionId);

    }
}
