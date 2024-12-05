using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.DAL.Models
{
    public class Rubrics:BaseEntity
    {
        public int AssessmentID { get; set; } // foreign key with Assessments table 
        public string Criterion { get; set; }
        public int MaxPoints { get; set; }
        virtual public Assessments Assessments { get; set; }
    }
}
