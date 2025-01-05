using AutoMapper;
using Educational_System.Dto.Lesson;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public LessonController(ILessonRepository lessonRepository, IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        // 1. Get all lessons with optional filters
        [HttpGet]
        public async Task<IActionResult> GetAll(int? sublessonId = null, string title = null)
        {
            var lessons = await _lessonRepository.GetLessonsAsync(sublessonId, title);
            var lessonsInfo = _mapper.Map<List<getLessonDto>>(lessons);
            return Ok(lessonsInfo);
        }

        // 2. Get lessons by sublesson ID
        [HttpGet("sublesson/{sublessonId}")]
        public async Task<IActionResult> GetLessonsBySublesson(int sublessonId)
        {
            var lessons = await _lessonRepository.GetLessonsBySublessonAsync(sublessonId);
            var lessonsInfo = _mapper.Map<List<getLessonDto>>(lessons);

            if (lessonsInfo == null || !lessonsInfo.Any())
                return NotFound("No lessons found for the specified sublesson.");

            return Ok(lessonsInfo);
        }

        // 3. Get a specific lesson with prerequisites
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lesson = await _lessonRepository.GetLessonWithPrerequisitesAsync(id);
            var lessonInfo = _mapper.Map<getLessonDto>(lesson);

            if (lessonInfo == null)
                return NotFound("Lesson not found.");

            return Ok(lessonInfo);
        }

        // 4. Add a prerequisite to a lesson
        [HttpPost("AddPrerequisite")]
        public async Task<IActionResult> AddPrerequisite([FromBody] Lesson_Prerequisites prerequisite)
        {
            if (prerequisite == null)
                return BadRequest("Prerequisite data is null.");

            var result = await _lessonRepository.AddPrerequisiteAsync(prerequisite.CurrentLessonID, prerequisite.PrerequisiteLessonID);
            if (!result)
                return BadRequest("Failed to add prerequisite.");

            return Ok(new { Message = "Prerequisite added successfully." });
        }

        // 5. Mark a lesson as completed for a user
        [HttpPost("MarkAsCompleted")]
        public async Task<IActionResult> MarkAsCompleted([FromBody] Lesson_Completions completion)
        {
            if (completion == null)
                return BadRequest("Completion data is null.");

            var result = await _lessonRepository.MarkLessonAsCompletedAsync(completion.LessonID, completion.UserID);
            if (!result)
                return BadRequest("Failed to mark lesson as completed.");

            return Ok(new { Message = "Lesson marked as completed." });
        }

        // 6. Check if a user has completed all prerequisites for a lesson
        [HttpGet("CheckPrerequisiteCompletion/{lessonId}/{userId}")]
        public async Task<IActionResult> CheckPrerequisiteCompletion(int lessonId, string userId)
        {
            var result = await _lessonRepository.CheckPrerequisiteCompletionAsync(lessonId, userId);
            return Ok(new { AllPrerequisitesMet = result });
        }

        // 7. Get lessons for a sublesson, ordered by prerequisite completion
        [HttpGet("OrderedByPrerequisite/{sublessonId}/{userId}")]
        public async Task<IActionResult> GetLessonsOrderedByPrerequisite(int sublessonId, string userId)
        {
            var lessons = await _lessonRepository.GetLessonsOrderedByPrerequisiteAsync(sublessonId, userId);
            var lessonsInfo = _mapper.Map<List<getLessonDto>>(lessons);

            return Ok(lessonsInfo);
        }
        // Add a lesson to a course
        [HttpPost("AddLessonToCourse/{courseId}")]
        public async Task<IActionResult> AddLessonToCourse(int courseId, [FromBody] postLessonDto lessonDto)
        {
            if (lessonDto == null)
                return BadRequest("Lesson data is null.");

            var lesson = _mapper.Map<postLessonDto, Lessons>(lessonDto);

            await _lessonRepository.AddLessonToCourseAsync(lesson, courseId);
            return CreatedAtAction(nameof(GetById), new { id = lesson.ID }, lesson);
        }

        
        // Delete a lesson by ID
        [HttpDelete("DeleteLesson/{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
                return NotFound("Lesson not found.");

            await _lessonRepository.DeleteLessonAsync(id);
            return NoContent();
        }
    }
}