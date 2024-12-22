using AutoMapper;
using Educational_System.Dto.Category;
using Educational_System.Dto.Course;
using EducationalSystem.DAL.Models;

namespace Educational_System.Helpers
{
    public partial class MappingProfiles : Profile
    {
        private void CreateCourseMappings()
        {
            CreateMap<Courses, getCourseDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.CourseTitle))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Categories.CategoryName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ThumbnailURL, opt => opt.MapFrom(src => src.ThumbnailURL))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Course_Instructors.Instructors.applicationUser.Name))
                .ForMember(dest => dest.DiscountStartDate, opt => opt.MapFrom(src => src.Discounts != null && src.Discounts.Any()
                    ? src.Discounts.Min(d => d.StartDate)
                    : (DateTime?)null))
                .ForMember(dest => dest.DiscountEndDate, opt => opt.MapFrom(src => src.Discounts != null && src.Discounts.Any()
                    ? src.Discounts.Max(d => d.EndDate)
                    : (DateTime?)null))
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.Discounts != null && src.Discounts.Any()
                    ? src.Discounts.Max(d => d.DiscountValue) 
                    : 0))
                .ForMember(dest => dest.SubLessons, opt => opt.MapFrom(src => src.SubLessons.Select(sub => new CourseSubLessons
                {
                    Title = sub.Title,
                    Description = sub.Description,
                    CreatedDate = sub.CreatedDate,
                    Lessons = sub.Lessons != null
                        ? sub.Lessons.Select(lesson => new CourseLessons
                        {
                            LessonTitle = lesson.LessonTitle,
                            Content = lesson.Content,
                            LessonOrder = lesson.LessonOrder,
                            LessonDescription = lesson.LessonDescription
                        }).ToList()
                        : new List<CourseLessons>()
                }).ToList()));


            CreateMap<Courses, postCourseDto>()
                .ReverseMap();

            CreateMap<Courses, getEnrolledStudentsDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.CourseTitle))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Categories.CategoryName))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Course_Instructors.Instructors.applicationUser.Name))
                .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Course_Enrollments.Count))

                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Course_Enrollments.Select(s => new StudentsCourse
                {
                    StudentName = s.User.Name,
                    StudentEmail = s.User.Email
                }).ToList()));
        }
    }
}
