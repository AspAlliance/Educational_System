using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        // DO it
        private readonly IGenericRepository<Lessons> _genericRepository;
        private readonly IGenericRepository<Courses> _genericRepositoryCourse;
        private readonly ILessonRepository _lessonRepository;
        public LessonController(IGenericRepository<Lessons> genericRepository, ILessonRepository lessonRepository, 
            IGenericRepository<Courses> genericRepositoryComment)
        {
            _genericRepository = genericRepository;
            _lessonRepository = lessonRepository;
            _genericRepositoryCourse = genericRepositoryComment;
        }

        // Add new Lesson
        // issue --> what should do to crsId ??
        [HttpPost]
        public async Task<IActionResult> Add( [FromBody] LessonDTO lessonFromRequest)
        {
            if (lessonFromRequest == null)
            {
                return BadRequest("lesson data in required");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var lesson = new Lessons()
                {
                    LessonTitle = lessonFromRequest.LessonTitle,
                    LessonDescription = lessonFromRequest.LessonDescription,
                    Content = lessonFromRequest.Content,
                    CreatedDate = DateTime.Now, // created time 
                    CourseID = 1, // just for test
                    SubLessonID = 1, // just for test 
                };
                await _lessonRepository.AddAsync(lesson);
                return CreatedAtAction(
                nameof(GetById),
                new { id = lesson.ID },
                lessonFromRequest
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error {ex.Message}");
            }
        } // DTO For lesson to make the end user cannot see the structure of the lesson Table

        // Get All Lessons by CrsId with LessonDTO --> return comments in every lesson (:
        [HttpGet]
        public async Task<IActionResult> GetAllByCrsId(int crsId)
        {
            if (crsId <= 0)
            {
                return BadRequest("Invalid course ID");
            }
            var lessons = await _lessonRepository.GetLessonsByCrsIdAsync(crsId);
            if (!lessons.Any())
            {
                // Optionally log or handle the case when no lessons are found
                return NotFound($"No lessons found for CourseID: {crsId}");
            }
            var course = await _genericRepositoryCourse.GetByIdAsync(crsId);
            var crsName = course.CourseTitle;

            List<LessonCourseCommentDTO> lessonsListDTO = new List<LessonCourseCommentDTO>();
            foreach (var lesson in lessons)
            {
                var comments = await _lessonRepository.GetAllCommentsByLessonId(lesson.ID) ?? new List<Comments>();
                
                lessonsListDTO.Add(new LessonCourseCommentDTO
                {
                    crsName = crsName,
                    lessonTitle = lesson.LessonTitle,
                    lessonDescription = lesson.LessonDescription,
                    contect = lesson.Content,
                    createdDate = lesson.CreatedDate,
                    comments = comments,
                });
            }
            return Ok(lessonsListDTO);
            // this function return all lossons by crsId and return the commetns on the course and crsName 
            // then map all on lessonDTO
        }

        // Get Lesson & crsName & lessonComments by lessonId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid lesson ID");
            }
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                return NotFound($"No lessons found for ID: {id}");
            }
            var course = await _genericRepositoryCourse.GetByIdAsync(id);
            var crsName = course.CourseTitle;
            var comments = await _lessonRepository.GetAllCommentsByLessonId(id) ?? new List<Comments>();

            LessonCourseCommentDTO lessonDTO = new LessonCourseCommentDTO()
            {
                crsName = crsName,
                lessonTitle = lesson.LessonTitle,
                lessonDescription = lesson.LessonDescription,
                contect = lesson.Content,
                createdDate = lesson.CreatedDate,
                comments = comments,
            };
            return Ok(lessonDTO);
        }

        // Edit Lesson --> lessonDTO 
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, LessonDTO lessonFromRequst)
        {
            if (lessonFromRequst == null)
            {
                return BadRequest("lesson data is required.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
            { return NotFound("not found"); }

            try
            {
                lesson.LessonTitle = lessonFromRequst.LessonTitle;
                lesson.LessonDescription = lessonFromRequst.LessonDescription;
                lesson.CreatedDate = lessonFromRequst.CreatedDate;
                lesson.Content = lessonFromRequst.Content;

                await _lessonRepository.UpdateAsync(lesson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            // return created action
            return CreatedAtAction(
                nameof(GetById), // Action that retrieves the created resource
                new { id = id }, // Route values
                lessonFromRequst // The response body
                );
        } // this action edit to the lesson by lessonDTO 

        [HttpDelete]
        public async Task <IActionResult> Delete(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync (id);
            if (lesson == null)
                return NotFound("not found");

            try
            {
                await _lessonRepository.DeleteAsync(lesson);
            }
            catch(Exception ex)
            {
                return BadRequest($"An error occurred while deleting the lesson ID {id}: {ex.Message}");
            }
            return NoContent();
        }


    }
}
