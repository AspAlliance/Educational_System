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
    public class SpecializationsRepository : GenericReposiory<Specializations>, ISpecializationsRepository
    {
        private protected readonly Education_System _dbContext;
        public SpecializationsRepository(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<Specializations?> GetByNameAsync(string specializationName)
        {
            return await _dbContext.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationName == specializationName);
        }

        public async Task<Specializations> GetInstructorsBySpecializationIdAsync(int? instructorId)
        {
            return await _dbContext.Set<Instructors>()
                .Where(i => i.ID == instructorId)
                .Select(i => i.Specializations)
                .FirstOrDefaultAsync();
        }
    }
}
