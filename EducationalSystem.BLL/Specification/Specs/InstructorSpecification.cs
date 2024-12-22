using EducationalSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Specification.Specs
{
    //this should be class InstructorWithCoursesAndSpecializationsAndApplicationUserSpecification 
    //but to make it easier i named InstructorSpecification
    public class InstructorSpecification : BaseSpecification<Instructors>
    {
        public override IQueryable<Instructors> Apply(IQueryable<Instructors> query)
        {
            return query.Include(i => i.applicationUser)
                    .Include(i => i.Course_Instructors)
                    .ThenInclude(ci => ci.Courses)
                    .Include(i => i.Specializations);
        }
    }
}
