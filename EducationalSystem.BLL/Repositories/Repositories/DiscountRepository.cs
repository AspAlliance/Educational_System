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
    public class DiscountRepository : GenericReposiory<Discounts>, IDiscountRepository
    {

        private readonly Education_System _context;

        public DiscountRepository(Education_System context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Discounts>> GetDiscountsByCourseIdAsync(int courseId)
        {
            return await _context.Discounts
                .Where(d => d.CourseID == courseId)
                .ToListAsync();
        }
    }
}
