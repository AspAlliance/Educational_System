using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.DAL.Models
{
    public class TextSubmissions:BaseEntity
    {
        public int AssessmentID { get; set; } // foreign key with Assessments table
        public string UserID { get; set; } // foreign key with ApplicationUser table
        public DateTime SubmittedDate { get; set; }
        public string ResponseText { get; set; }
        public int Grade { get; set; }
        public string Feedback { get; set; }
        public Assessments Assessments { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
