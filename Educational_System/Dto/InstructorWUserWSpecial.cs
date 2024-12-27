using System.Text;

namespace Educational_System.Dto
{
    public class InstructorWUserWSpecial
    {
        public string Insname { get; set; } // from userTable
        public string InsphoneNumber { get; set; } // instructors
        public string InsNationalCardImg { get; set; }
        public string CV_PDF_URL { get; set; }
        public string BIO {  get; set; }

        public string specialName { get; set; } // Specializations table


        /*
          public string PhoneNumber { get; set; }
        public string CV_PDF_URL { get; set; }
        public string NationalCardImageURL { get; set; }
        public string BIO { get; set; }
         */

    }
}
