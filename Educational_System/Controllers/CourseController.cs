using AutoMapper;
using Educational_System.Dto.Category;
using Educational_System.Dto.Course;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.BLL.Specification.Specs;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;



namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IMemoryCache _cache1;
        private readonly ILogger<CourseController> _logger;
        private readonly IDistributedCache _cache;
        private static bool _redisDown = false;
        private static DateTime _lastRedisFail = DateTime.MinValue;
        private readonly IEnrollRepository _enrollRepository;
        public CourseController(
            ICourseRepository repository,
            IMapper mapper,
            IDistributedCache cache,
            IMemoryCache cache1,
            ILogger<CourseController> logger,
            IWebHostEnvironment environment,
            IEnrollRepository enrollRepository
            )
        {
            _courseRepository = repository;
            _mapper = mapper;
            _cache = cache;
            _cache1 = cache1;
            _logger = logger;
            _environment = environment;
            _enrollRepository = enrollRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stopwatch = Stopwatch.StartNew();
            string cacheKey = "all_courses";
            List<getCourseDto> data = null;

            // ⏳ تحديد مدة إعادة المحاولة مع Redis
            TimeSpan retryInterval = TimeSpan.FromMinutes(5);

            // 1️⃣ لو Redis مش معطّل، جرب تجيب منه
            if (!_redisDown || (DateTime.UtcNow - _lastRedisFail) > retryInterval)
            {
                try
                {
                    var cachedCourses = await _cache.GetStringAsync(cacheKey);
                    if (!string.IsNullOrEmpty(cachedCourses))
                    {
                        _logger.LogInformation("✅ البيانات من Redis");
                        data = JsonSerializer.Deserialize<List<getCourseDto>>(cachedCourses);
                        stopwatch.Stop();
                        return Ok(data);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Redis مش شغال - هنوقف المحاولة مؤقتًا");
                    _redisDown = true;
                    _lastRedisFail = DateTime.UtcNow;
                }
            }

            // 2️⃣ جرب من IMemoryCache
            if (_cache1.TryGetValue(cacheKey, out List<getCourseDto> memoryCacheData))
            {
                _logger.LogInformation("✅ البيانات من IMemoryCache");
                stopwatch.Stop();
                return Ok(memoryCacheData);
            }

            // 3️⃣ لو مفيش بيانات في الكاشين → نجيب من DB
            var spec = new CourseSpecification();
            var courses = await _courseRepository.GetAllWithSpec(spec);
            data = _mapper.Map<List<getCourseDto>>(courses);

            var serialized = JsonSerializer.Serialize(data);

            // نحاول نخزن في Redis (لو مش معطّل)
            if (!_redisDown || (DateTime.UtcNow - _lastRedisFail) > retryInterval)
            {
                try
                {
                    await _cache.SetStringAsync(cacheKey, serialized,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                        });
                    _logger.LogInformation("📦 البيانات اتحفظت في Redis");
                    _redisDown = false; // رجع يشتغل
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ فشل تخزين البيانات في Redis");
                    _redisDown = true;
                    _lastRedisFail = DateTime.UtcNow;
                }
            }

            // نخزن في IMemoryCache
            _cache1.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            _logger.LogInformation("📦 البيانات اتحفظت في IMemoryCache");

            stopwatch.Stop();
            return Ok(data);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddCourse([FromForm] postCourseDto courseDto)
        {
            if (courseDto is null)
                return BadRequest("Course is null.");

            var course = _mapper.Map<Courses>(courseDto);

            // رفع الصورة (اختياري)
            if (courseDto.Image is { Length: > 0 })
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(courseDto.Image.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await courseDto.Image.CopyToAsync(stream);

                // خيّار 1: URL نسبي (يفتح على نفس الدومين)
                // course.ThumbnailURL = $"/uploads/{fileName}";

                // خيّار 2: URL كامل — مناسب لو هتستهلكه من دومين مختلف
                course.ThumbnailURL = $"/uploads/{fileName}";

            }
            else
            {
                // صورة افتراضية (حط ملف default.jpg في wwwroot/uploads)
                course.ThumbnailURL = "/uploads/default.jpg";
            }

            await _courseRepository.AddAsync(course);
            return CreatedAtAction(nameof(GetById), new { id = course.ID }, course);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] postCourseDto courseDto)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse is null)
                return NotFound("Course not found.");

            // تحديث الحقول الاختيارية
            if (!string.IsNullOrWhiteSpace(courseDto.CourseTitle))
                existingCourse.CourseTitle = courseDto.CourseTitle;

            if (!string.IsNullOrWhiteSpace(courseDto.Description))
                existingCourse.Description = courseDto.Description;

            if (courseDto.CategoryID.HasValue)
                existingCourse.CategoryID = courseDto.CategoryID.Value;

            if (courseDto.duration.HasValue)
                existingCourse.duration = courseDto.duration.Value;

            if (courseDto.CreatedDate.HasValue)
                existingCourse.CreatedDate = courseDto.CreatedDate.Value;

            if (courseDto.TotalAmount.HasValue)
                existingCourse.TotalAmount = courseDto.TotalAmount.Value;

            if (!string.IsNullOrWhiteSpace(courseDto.level))
                existingCourse.level = courseDto.level;

            // لو فيه صورة جديدة – احفظ واستبدل الرابط
            if (courseDto.Image is { Length: > 0 })
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(courseDto.Image.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await courseDto.Image.CopyToAsync(stream);

                // نخزن المسار النسبي فقط
                existingCourse.ThumbnailURL = $"/uploads/{fileName}";
            }

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

        // enroll course by user id 
        [HttpPost("enroll/{id}")]
        public async Task<IActionResult> EnrollInCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return NotFound("Course not found.");
            // Assuming you have a way to get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");
            // Check if the user is already enrolled


            var enroll = await _enrollRepository.EnrollInCourseAsync(id, userId);


            if (!enroll)
                return BadRequest("User is already enrolled in this course.");

            return Ok("Successfully enrolled in the course.");
        }

        // get the user enrolled courses
        [Authorize]
        [HttpGet("user/enrolled")]
        public async Task<IActionResult> GetUserEnrolledCourses()
        {
            // Assuming you have a way to get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");
            var enrolledCourses = await _enrollRepository.GetUserEnrolledCoursesAsync(userId);
            if (enrolledCourses == null || !enrolledCourses.Any())
                return NotFound("No enrolled courses found for this user.");
            var enrolledCoursesInfo = _mapper.Map<List<getCourseDto>>(enrolledCourses);
            return Ok(enrolledCoursesInfo);
        }
    }
}
