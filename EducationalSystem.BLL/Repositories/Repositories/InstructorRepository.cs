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
    public class InstructorRepository : GenericReposiory<Instructors>, IInstructorRepository
    {
        private protected readonly Education_System _dbContext;
        public InstructorRepository(Education_System dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IEnumerable<ApplicationUser>> GeInstructorUsersAsync(int instructorId)
        {
            var users = await _dbContext.Users
                .Where(user => user.User_Instructor.Any(ui => ui.InstructorId == instructorId))
                .AsNoTracking()
                .ToListAsync();

            return users.AsEnumerable(); // Convert List to IEnumerable
        }


    }
}
