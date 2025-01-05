using EducationalSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Specification.Specs
{
    public class SpecializationSpecification : BaseSpecification<Specializations>
    {
        public override IQueryable<Specializations> Apply(IQueryable<Specializations> query)
        {
            return query.Include(s => s.Instructors).ThenInclude(i => i.applicationUser);
        }
    }
}
