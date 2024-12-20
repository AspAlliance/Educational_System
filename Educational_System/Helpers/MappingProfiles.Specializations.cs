using AutoMapper;
using Educational_System.Dto.Category;
using Educational_System.Dto.Specialization;
using EducationalSystem.DAL.Models;

namespace Educational_System.Helpers
{
    public partial class MappingProfiles : Profile
    {
        private void CreateSpecializatiosMappings()
        {
            CreateMap<Specializations, getSpecializationDto>()
                .ForMember(dest => dest.InstructorCount, opt => opt.MapFrom(src => src.Instructors.Count))
                .ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.Instructors.Select(cs => new InstructorsSpecialization
                {
                    ID = cs.ID,
                    Name = cs.applicationUser.Name
                }).ToList()));

            CreateMap<Specializations, postSpecializationDto>()
                .ReverseMap();
        }
    }
}
