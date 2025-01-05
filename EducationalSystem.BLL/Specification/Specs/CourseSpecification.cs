using EducationalSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EducationalSystem.BLL.Specification.Specs
{
    public class CourseSpecification : BaseSpecification<Courses>
    {
        public CourseSpecification()
        {

        }
        public CourseSpecification(string search) : base(c =>
                string.IsNullOrEmpty(search) ||
                c.CourseTitle.Contains(search) ||
                c.Description.Contains(search) ||
                c.Categories.CategoryName.Contains(search) ||
                c.Course_Instructors.Instructors.applicationUser.Name.Contains(search))
        {
                
        }
        public CourseSpecification(decimal? minPrice, decimal? maxPrice, string instructor, int? categoryId)
        : base(c =>
            (!minPrice.HasValue || c.TotalAmount >= minPrice) &&
            (!maxPrice.HasValue || c.TotalAmount <= maxPrice) &&
            (string.IsNullOrEmpty(instructor) || c.Course_Instructors.Instructors.applicationUser.Name == instructor) &&
            (!categoryId.HasValue || c.Categories.ID == categoryId))
        {

        }
        public override IQueryable<Courses> Apply(IQueryable<Courses> query)
        {
            query = base.Apply(query);

            return query
                .Include(c => c.Discounts)
                .Include(c => c.Categories)
                .Include(c => c.SubLessons).ThenInclude(s => s.Lessons)
                .Include(c => c.Course_Instructors).ThenInclude(ci => ci.Instructors).ThenInclude(i => i.applicationUser)
                .Include(c => c.Course_Enrollments).ThenInclude(ce => ce.User);
        }
    }
}
