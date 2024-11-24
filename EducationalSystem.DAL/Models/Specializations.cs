using System.ComponentModel.DataAnnotations;

namespace EducationalSystem.DAL.Models
{
    public class Specializations : BaseEntity
    {
        public string SpecializationName { get; set; }
        public ICollection<Instructors> Instructors { get; set; }

    }
}
