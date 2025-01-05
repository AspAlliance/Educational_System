using EducationalSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Specification.Specs
{
    public class CategorySpecification : BaseSpecification<Categories>
    {
        public override IQueryable<Categories> Apply(IQueryable<Categories> query)
        {
            return query.Include(c => c.Courses);
        }
    }
}
