using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.DAL.Models
{
    public class SubLessons:BaseEntity
    {
        public int CourseID { get; set; } // foreign key with courses
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        virtual public ICollection<Lessons>? Lessons { get; set; }
        virtual public Courses Courses { get; set; }
    }
}
