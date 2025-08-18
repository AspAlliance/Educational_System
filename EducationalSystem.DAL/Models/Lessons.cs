namespace EducationalSystem.DAL.Models
{
    public class Lessons : BaseEntity
    {
        public int CourseID { get; set; }  // Foreign key for Courses
        public string LessonTitle { get; set; }
        public string Content { get; set; }
        public int LessonOrder { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int SubLessonID { get; set; } // Foreign key for Courses
        public string LessonDescription { get; set; }
        public string? VideoUrl { get; set; } // URL for the lesson video
        public int? Duration { get; set; }// Duration of the lesson, e.g., "1 hour 30 minutes"



        // Inverse navigation for CurrentLesson
        virtual public ICollection<Lesson_Prerequisites> CurrentLessonPrerequisites { get; set; }

        // Inverse navigation for PrerequisiteLesson
        virtual public ICollection<Lesson_Prerequisites> PrerequisiteLessonPrerequisites { get; set; }
        virtual public ICollection<Assessments>? Assessments { get; set; }
        virtual public ICollection<Comments> Comments { get; set; }
        virtual public ICollection<Lesson_Completions> Lesson_Completions { get; set; }

        // Navigation property for Courses
        virtual public Courses Courses { get; set; }
        virtual public SubLessons SubLessons { get; set; }
    }
}
/*
 *    this.course = {
          id: res.id,
          title: res.title,
          category: res.category ?? 'Web Development',
          breadcrumb: ['Courses', res.category ?? 'General', res.title],
          lessons: res.subLessons?.reduce((acc: number, s: any) => acc + (s.lessons?.length ?? 0), 0) ?? 0,
          duration: 'N/A', // لو عندك duration في الـ API تقدر تحطه هنا
          rating: res.rating ?? 5,
          reviews: res.reviews ?? 0,
          videoUrl: res.videoUrl ?? 'https://www.w3schools.com/html/mov_bbb.mp4',
          description: res.description,
          whatYouLearn: res.whatYouLearn ?? [],
          content: res.subLessons?.map((s: any) => ({
            section: s.title,
            duration: s.duration ?? '—',
            lessons: s.lessons?.map((l: any) => ({
              title: l.lessonTitle,
              time: l.duration ?? '—',
              preview: l.preview ?? false,
            })) ?? [],
          })) ?? [],
          author: res.author ?? {
            name: 'Unknown',
            role: 'Instructor',
            image: 'assets/person2.jpeg',
          },
     
*/   