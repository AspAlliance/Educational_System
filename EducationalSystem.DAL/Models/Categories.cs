using System.Collections.Generic;

namespace EducationalSystem.DAL.Models
{
    public class Categories : BaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<Courses> Courses { get; set; }
    }
}
