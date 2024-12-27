using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationalSystem.DAL.Models;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface ISpecializationsRepository: IGenericRepository<Specializations>
    {
        Task<Specializations?> GetByNameAsync(string specializationName);
        public Task<Specializations> GetInstructorsBySpecializationIdAsync(int? instructorId);
    }


}
