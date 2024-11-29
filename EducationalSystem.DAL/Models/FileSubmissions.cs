using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.DAL.Models
{
    public class FileSubmissions:BaseEntity
    {
        public int AssessmentID { get; set; }
        public string UserID { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string FilePath { get; set; }
        public int Grade { get; set; }
        public string Feedback { get; set; }
        public Assessments Assessments { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
