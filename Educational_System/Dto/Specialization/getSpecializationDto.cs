using Educational_System.Dto.Category;

namespace Educational_System.Dto.Specialization
{
    public class InstructorsSpecialization
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class getSpecializationDto
    {
        public int Id { get; set; }
        public string SpecializationName { get; set; }
        public string Description { get; set; }
        public int InstructorCount { get; set; }
        public List<InstructorsSpecialization> Instructors { get; set; }
    }
}
