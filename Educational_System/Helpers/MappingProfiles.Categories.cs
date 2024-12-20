using AutoMapper;
using Educational_System.Dto.Category;
using EducationalSystem.DAL.Models;

namespace Educational_System.Helpers
{
    public partial class MappingProfiles : Profile
    {
        private void CreateCategoriesMappings()
        {
            CreateMap<Categories, getCategoryDto>()
                .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count))
                .ForMember(dest => dest.courses, opt => opt.MapFrom(src => src.Courses.Select(cs => new CourseCategory
                {
                    ID = cs.ID,
                    CourseTitle = cs.CourseTitle
                }).ToList()));

            CreateMap<Categories, postCategoryDto>()
                .ReverseMap();
        }
    }
}
