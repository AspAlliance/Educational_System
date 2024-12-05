using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.DAL.Models;
using System.Net;

namespace Educational_System.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Assessments, AssessmentsDto>()
                .ReverseMap();
            CreateMap<RegisterBS, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Instructors, InstructorsDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.applicationUser.Name))
                .ForMember(dest => dest.SpecializationsName, opt => opt.MapFrom(src => src.Specializations.SpecializationName)) 
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Course_Instructors.Select(ci => ci.Courses).ToList()))
              //.ForMember(dest => dest.CourseTitles, opt => opt.MapFrom(src => src.Course_Instructors.Select(ci => ci.Courses.CourseTitle).ToList()))
                .ReverseMap();



        }
    }
}
