using AutoMapper;
using Educational_System.Dto.Category;
using Educational_System.Dto.Course;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.BLL.Specification.Specs;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository repository, IMapper mapper)
        {
            _courseRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spec = new CourseSpecification();
            var courses = await _courseRepository.GetAllWithSpec(spec);
            var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);
            return Ok(coursesInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var spec = new CourseSpecification();
            var course = await _courseRepository.GetByIdWithSpecAsync(id, spec);
            var courseInfo = _mapper.Map<getCourseDto>(course);
            if (courseInfo == null)
                return NotFound("Course not found.");

            return Ok(courseInfo);
        }
        //Instructor Only)
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] postCourseDto courseDto)
        {

            var mappedCourse = _mapper.Map<postCourseDto, Courses>(courseDto);

            if (mappedCourse == null)
                return BadRequest("Course is null.");

            await _courseRepository.AddAsync(mappedCourse);
            return CreatedAtAction(nameof(GetById), new { id = mappedCourse.ID }, mappedCourse);
        }
        //Instructor Only
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] postCourseDto courseDto)
        {
            if (courseDto == null)
                return BadRequest("Course data is null.");

            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFound("Course not found.");

            _mapper.Map(courseDto, existingCourse);

            await _courseRepository.UpdateAsync(existingCourse);
            return NoContent();
        }
        //Instructor or Admin Only)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return NotFound("Course not found.");

            await _courseRepository.DeleteAsync(course);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses([FromQuery] string? query)
        {
            var spec = new CourseSpecification(query);
            var courses = await _courseRepository.GetAllWithSpec(spec);
            var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);

            return Ok(coursesInfo);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterCourses(decimal? minPrice, decimal? maxPrice, string? instructor, int? categoryId)
        {
            var spec = new CourseSpecification(minPrice, maxPrice, instructor, categoryId);
            var courses = await _courseRepository.GetAllWithSpec(spec);
            var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);

            return Ok(coursesInfo);
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetEnrolledStudents(int id)
        {

            var spec = new CourseSpecification();
            var course = await _courseRepository.GetByIdWithSpecAsync(id, spec);
            var courseInfo = _mapper.Map<getEnrolledStudentsDto>(course);
            if (courseInfo == null)
                return NotFound("Course not found.");

            return Ok(courseInfo);
        }

        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetInstructorCourses(int instructorId)
        {
            var spec = new CourseSpecification();
            var instructorCourses = await _courseRepository.GetCoursesByInstructorWithSpecsAsync(instructorId, spec);
            var instructorCoursesInfo = _mapper.Map<List<getCourseDto>>(instructorCourses);
            return Ok(instructorCoursesInfo);
        }
    }
}
