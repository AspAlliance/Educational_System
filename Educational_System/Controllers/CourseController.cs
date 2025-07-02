using AutoMapper;
using Educational_System.Dto.Category;
using Educational_System.Dto.Course;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.BLL.Specification.Specs;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol.Core.Types;
using System.Text.Json;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;

        private readonly ILogger<CourseController> _logger;
        //private readonly IDistributedCache _cache;

        public CourseController(
            ICourseRepository repository,
            IMapper mapper,
          //   IDistributedCache cache,
            IMemoryCache cache,
            ILogger<CourseController> logger)
        {
            _courseRepository = repository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var spec = new CourseSpecification();
        //    var courses = await _courseRepository.GetAllWithSpec(spec);
        //    var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);
        //    return Ok(coursesInfo);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string cacheKey = "all_courses";

            if (!_cache.TryGetValue(cacheKey, out List<getCourseDto> cachedCourses))
            {
                _logger.LogInformation("⛔ الكاش فاضي، جاري تحميل الدورات من قاعدة البيانات...");

                var spec = new CourseSpecification();
                var courses = await _courseRepository.GetAllWithSpec(spec);
                cachedCourses = _mapper.Map<List<getCourseDto>>(courses);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, cachedCourses, cacheEntryOptions);

                _logger.LogInformation("✅ تم تخزين الدورات في الكاش.");
            }
            else
            {
                _logger.LogInformation("✅ تم جلب الدورات من الكاش.");
            }

            return Ok(cachedCourses);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    string cacheKey = "all_courses";

        //    var cachedCourses = await _cache.GetStringAsync(cacheKey);
        //    if (!string.IsNullOrEmpty(cachedCourses))
        //    {
        //        _logger.LogInformation("✅ الكاش جاب البيانات (Redis)");
        //        _logger.LogInformation("🎯 CourseController.GetAll has been called at {Time}", DateTime.Now);
        //        var data = JsonSerializer.Deserialize<List<getCourseDto>>(cachedCourses);
        //        return Ok(data);
        //    }

        //    var spec = new CourseSpecification();
        //    var courses = await _courseRepository.GetAllWithSpec(spec);
        //    var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);

        //    var serialized = JsonSerializer.Serialize(coursesInfo);
        //    await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
        //    {
        //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        //    });

        //    _logger.LogInformation("⛔ الكاش فاضي - جبت من الداتابيز وخزنت في Redis");

        //    return Ok(coursesInfo);
        //}


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

        [HttpPost("simplify")]
        public IActionResult SimplifyJson([FromBody] EchoJson original)
        {
            var result = new
            {
                LV = $"{original.LV.dimension}, {original.LV.systolic} systolic, {original.LV.diastolic}",
                RV = $"{original.RV.dimension}, {original.RV.systolic} systolic",
                Atrium = original.Atrium,
                AV = $"{original.AV.leaflets}",
                TV = $"{original.TV.dimension}, {UpperFirst(original.TV.regurge)} regurge",
                pacemaker = original.pacemaker
            };

            return Ok(result);
        }

        private string UpperFirst(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1);
        }

        // الكلاسات المساعدة
        public class EchoJson
        {
            public LVSection LV { get; set; }
            public RVSection RV { get; set; }
            public string Atrium { get; set; }
            public AVSection AV { get; set; }
            public TVSection TV { get; set; }
            public string WM { get; set; }
            public string pacemaker { get; set; }
            public string Pericardium { get; set; }
        }

        public class LVSection
        {
            public string dimension { get; set; }
            public string systolic { get; set; }
            public string diastolic { get; set; }
            public string wall_thickness { get; set; }
        }

        public class RVSection
        {
            public string dimension { get; set; }
            public string systolic { get; set; }
        }

        public class AVSection
        {
            public string leaflets { get; set; }
            public string regurge { get; set; }
        }

        public class TVSection
        {
            public string dimension { get; set; }
            public string regurge { get; set; }
        }


    //    [HttpGet("filter")]
    //    public async Task<IActionResult> FilterCourses(decimal? minPrice, decimal? maxPrice, string? instructor, int? categoryId)
    //    {
    //        var spec = new CourseSpecification(minPrice, maxPrice, instructor, categoryId);
    //        var courses = await _courseRepository.GetAllWithSpec(spec);
    //        var coursesInfo = _mapper.Map<List<getCourseDto>>(courses);

    //        return Ok(coursesInfo);
    //    }

    //    [HttpGet("{id}/students")]
    //    public async Task<IActionResult> GetEnrolledStudents(int id)
    //    {

    //        var spec = new CourseSpecification();
    //        var course = await _courseRepository.GetByIdWithSpecAsync(id, spec);
    //        var courseInfo = _mapper.Map<getEnrolledStudentsDto>(course);
    //        if (courseInfo == null)
    //            return NotFound("Course not found.");

    //        return Ok(courseInfo);
    //    }

    //    [HttpGet("instructor/{instructorId}")]
    //    public async Task<IActionResult> GetInstructorCourses(int instructorId)
    //    {
    //        var spec = new CourseSpecification();
    //        var instructorCourses = await _courseRepository.GetCoursesByInstructorWithSpecsAsync(instructorId, spec);
    //        var instructorCoursesInfo = _mapper.Map<List<getCourseDto>>(instructorCourses);
    //        return Ok(instructorCoursesInfo);
    //    }
    }
}
