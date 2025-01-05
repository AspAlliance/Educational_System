using AutoMapper;
using Educational_System.Dto.Lesson;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Educational_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubLessonController : ControllerBase
    {
        private readonly ISubLessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        public SubLessonController(ISubLessonRepository subLessonRepository, IMapper mapper) 
        {
            _lessonRepository = subLessonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sublesson = await _lessonRepository.GetAll();
            return Ok(sublesson);
        }
        // Add a sublesson to a course
        [HttpPost("AddSubLessonToCourse/{courseId}")]
        public async Task<IActionResult> AddSubLessonToCourse(int courseId, [FromBody] postSubLessonDto subLessonDto)
        {
            if (subLessonDto == null)
                return BadRequest("Sublesson data is null.");

            var subLesson = _mapper.Map<postSubLessonDto, SubLessons>(subLessonDto);

            await _lessonRepository.AddSubLessonToCourseAsync(subLesson, courseId);
            return Ok(new { Message = "Sublesson added to course." });
        }

        //    [HttpGet]
        //    public IActionResult GetById(int id)
        //    {
        //        return Ok("Get sublesson by ID");
        //    }
        //    [HttpPost]
        //    public IActionResult AddSubLesson()
        //    {
        //        return Ok("Add a sublesson");
        //    }
        //    [HttpPut]
        //    public IActionResult UpdateSubLesson()
        //    {
        //        return Ok("Update a sublesson");
        //    }
        //    [HttpDelete]
        //    public IActionResult DeleteSubLesson()
        //    {
        //        return Ok("Delete a sublesson");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByLesson(int lessonId)
        //    {
        //        return Ok("Get sublessons by lesson ID");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByCourse(int courseId)
        //    {
        //        return Ok("Get sublessons");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByUser(string userId)
        //    {
        //        return Ok("Get sublessons by user");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByCourseAndUser(int courseId, string userId)
        //    {
        //        return Ok("Get sublessons by course and user");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByLessonAndUser(int lessonId, string userId)
        //    {
        //        return Ok("Get sublessons by lesson and user");
        //    }
        //    [HttpGet]
        //    public IActionResult GetSubLessonsByCourseAndLesson(int courseId, int lessonId)
        //    {
        //        return Ok("Get sublessons");
        //    }
        //    [HttpPost]
        //    public IActionResult AddSubLessonToCourse()
        //    {
        //        return Ok("Add a sublesson to a course");
        //    }
    }
}
