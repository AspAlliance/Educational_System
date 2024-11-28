using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _LessonRepository;

        public LessonController(ILessonRepository LessonRepository)
        {
            _LessonRepository = LessonRepository;
        }

        [HttpGet]
        public IActionResult GetAllLessons()
        {
            var Lessons = _LessonRepository.GetAll();
            return Ok(Lessons);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLessonsById(int id)
        {
            var LessonByID = await _LessonRepository.GetByIdAsync(id);
            return Ok(LessonByID);
        }
    }
}
