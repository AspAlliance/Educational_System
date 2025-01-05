using AutoMapper;
using Educational_System.Dto;
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
            CreateMap<Assessments, AssessmentsDto>()
                .ReverseMap();
            CreateMap<RegisterBS, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Instructors, InstructorsDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.applicationUser.Name))
                .ForMember(dest => dest.SpecializationsName, opt => opt.MapFrom(src => src.Specializations.SpecializationName))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Course_Instructors.Select(ci => ci.Courses).ToList()))
                .ReverseMap();

            CreateCategoriesMappings();

            CreateSpecializatiosMappings();

            CreateCourseMappings();

            // Added mappings for Choices
            CreateMap<Choices, GetChoicesDto>();
            CreateMap<PostChoicesDto, Choices>();
        }
    }
}