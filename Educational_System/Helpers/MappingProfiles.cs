using AutoMapper;
using Educational_System.Dto;
using Educational_System.Dto.Category;
using Educational_System.Dto.Choices;
using Educational_System.Dto.Lesson;
using Educational_System.Dto.Category;
using Educational_System.Dto.Choices; // Added namespace for Choices DTOs
using EducationalSystem.DAL.Models;
using System.Net;

namespace Educational_System.Helpers
{
    public partial class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Assessments mapping
            CreateMap<Assessments, AssessmentsDto>()
                .ReverseMap();

            // User registration mapping
            CreateMap<RegisterBS, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            // Instructors mapping
            CreateMap<Instructors, InstructorsDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.applicationUser.Name))
                .ForMember(dest => dest.SpecializationsName, opt => opt.MapFrom(src => src.Specializations.SpecializationName))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Course_Instructors.Select(ci => ci.Courses).ToList()))
                .ReverseMap();

            // Categories mapping
            CreateCategoriesMappings();

            // Specializations mapping
            CreateSpecializatiosMappings();
            CreateCategoriesMappings();

            CreateSpecializatiosMappings();

            // Course mappings
            CreateCourseMappings();
            CreateCourseMappings();

            // Choices mapping
            CreateMap<Choices, GetChoicesDto>();
            CreateMap<PostChoicesDto, Choices>();

            // Lesson mappings
            CreateMap<postLessonDto, Lessons>()
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.LessonTitle))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.LessonOrder, opt => opt.MapFrom(src => src.LessonOrder))
                .ForMember(dest => dest.LessonDescription, opt => opt.MapFrom(src => src.LessonDescription))
                .ForMember(dest => dest.SubLessonID, opt => opt.MapFrom(src => src.SubLessonID))
                .ReverseMap(); // Add reverse mapping if needed

            CreateMap<Lessons, getLessonDto>()
                .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.PrerequisiteLessonPrerequisites))
                .ReverseMap();

            CreateMap<Lesson_Prerequisites, getLessonPrerequisiteDto>()
                .ReverseMap();

            CreateMap<SubLessons, postSubLessonDto>()
                .ReverseMap();
            CreateMap<TextSubmissions, TextSubmissionDTO>().ReverseMap();
            // Added mappings for Choices
            CreateMap<Choices, GetChoicesDto>();
            CreateMap<PostChoicesDto, Choices>();
        }
    }
}