using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly IGenericRepository<Lessons> _genericRepositoryLesson;
        private readonly IGenericRepository<Courses> _genericRepositoryCourse;
        private readonly IGenericRepository<SubLessons> _genericRepositorySubLesson;
        private readonly IGenericRepository<Lesson_Prerequisites> _genericlessonPrerequisites;
        private readonly IGenericRepository<Lesson_Completions> _genericlessonCompletions;

        private readonly ILessonRepository _lessonRepository;
        public LessonController(IGenericRepository<Lessons> genericRepository,
            ILessonRepository lessonRepository, 
            IGenericRepository<Courses> genericRepositoryCourse, 
            IGenericRepository<SubLessons> genericRepositorySubLesson,
            IGenericRepository<Lesson_Prerequisites> lessonPrerequisites,
            IGenericRepository<Lesson_Completions> genericlessonCompletions)
        {
            _genericRepositoryLesson = genericRepository;
            _lessonRepository = lessonRepository;
            _genericRepositoryCourse = genericRepositoryCourse;
            _genericRepositorySubLesson = genericRepositorySubLesson;
            _genericlessonPrerequisites = lessonPrerequisites;
            _genericlessonCompletions = genericlessonCompletions;
        }
         
        // 1. Add Lesson to Sublesson ()
        // Roles Allowed: Instructor, Admin <--
        [HttpPost("sublessons/{subLessonId}/lessons")]
        public async Task<IActionResult> Add(int subLessonId, [FromBody] LessonDTO lessonFromRequest)
        {
            if (lessonFromRequest == null)
            {
                return BadRequest("lesson data in required");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subLesson = await _genericRepositorySubLesson.GetByIdAsync(subLessonId);
            if (subLesson == null)
            {
                return NotFound($"SubLesson with id {subLessonId} not found");
            }
            try
            {
                var lesson = new Lessons()
                {
                    LessonTitle = lessonFromRequest.LessonTitle,
                    LessonDescription = lessonFromRequest.LessonDescription,
                    Content = lessonFromRequest.Content,
                    LessonOrder = lessonFromRequest.lessonOrder,
                    CreatedDate = DateTime.UtcNow, // created time--> UtcNow-> if application is distributed across different time zones
                    CourseID = subLesson.CourseID, // just for test
                    SubLessonID = subLessonId, // just for test 
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
                return StatusCode(500, $"internal server error ");
            }
        } // DTO For lesson to make the end user cannot see the structure of the lesson Table

        // 2. Get All Lessons by Sublesson 
        [HttpGet("sublessons/{subLessonId}/lessons")]
        public async Task<IActionResult> GetAllBySublesson(int subLessonId)
        {
            try
            {
                if (subLessonId <= 0)
                {
                    return BadRequest("Invalid course ID");
                }
                var lessons = await _lessonRepository.GetLessonsBySubLessonIdAsync(subLessonId);
                if (!lessons.Any())
                {
                    // Optionally log or handle the case when no lessons are found
                    return NotFound($"No lessons found for CourseID: {subLessonId}");
                }
                var course = await _genericRepositoryCourse.GetByIdAsync(subLessonId);
                if (course == null)
                {
                    return NotFound($"Course with ID: {subLessonId} not found");
                }
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
            }

            // this function return all lossons by crsId and return the commetns on the course and crsName 
            // then map all on lessonDTO
            catch (Exception ex)
            {
                return StatusCode(500, $"an error occurred while processing your request, {ex.Message}");
            }
        }

        // Get Lesson & crsName & lessonComments by lessonId
        // 3. Get Specific Lesson 
        [HttpGet("lessons/{id}")]
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
        // 4. Update Lesson 
        [HttpPut("lessons/{id}")]
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
                lesson.LessonOrder = lessonFromRequst.lessonOrder;

                await _lessonRepository.UpdateAsync(lesson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while processing your request.");
            }
            return CreatedAtAction(
               nameof(GetById), // Action that retrieves the created resource
               new { id = id }, // Route values

               lessonFromRequst // The response body
               );
        } // this action edit to the lesson by lessonDTO 

        // 5. Delete Lesson
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
                return NotFound("not found");

            try
            {
                await _lessonRepository.DeleteAsync(lesson);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the lesson ID {id}");
            }
            return NoContent();
        }

        // 6. Add Prerequisite to Lesson
        [HttpPost("lessons/{lessonId}/prerequisites")]
        public async Task<IActionResult> AddPrerequisite(int lessonId, [FromBody] AddPrerequisiteDto prerequisiteDto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                return NotFound("lesson not found");

            var lessonpreqFromDb = await _lessonRepository.GetByIdAsync(prerequisiteDto.prerequisiteLessonId);
            if (lessonpreqFromDb == null)
            {
                return NotFound("Prerequisite lesson not found");
            }

            var prerequisiteLesson = await _lessonRepository.GetLessonPrerequisitesByIdAsync(lessonId);
            
            // Check if the prerequisite already exists
            foreach (var item in prerequisiteLesson)
            {
                if (prerequisiteDto.prerequisiteLessonId == item.PrerequisiteLessonID)
                {
                    return Conflict(new { error = "Prerequisite already exists for this lesson." });
                }
            }
            if (prerequisiteLesson == null)
            {
                return BadRequest(new { error = "Invalid prerequisite lesson ID." });
            }

            var lesson_prereuiste = new Lesson_Prerequisites
            {
                CurrentLessonID = lessonId,
                PrerequisiteLessonID = prerequisiteDto.prerequisiteLessonId,
            };
            try
            {
                await _genericlessonPrerequisites.AddAsync(lesson_prereuiste);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while processing your request.");
            }
            return CreatedAtAction(
                nameof(GetById),
                new { id = lessonId },
                prerequisiteDto
                );
        }

        // 7. Get Lesson Prerequisites
        [HttpGet("lessons/{lessonId}/prerequisites")]
        public async Task<IActionResult> GetLessonPrerequisites(int lessonId)
        {
            try
            {
                var lessonPrerequisites = await _lessonRepository.GetLessonPrerequisitesByIdAsync(lessonId);
                // list of Lesson_Prerequisites
                if (lessonPrerequisites.Count == 0)
                {
                    return NotFound("there is no prerequisites for this lesson.");
                }
                var prerequisiteIds = lessonPrerequisites.Select(lp => lp.PrerequisiteLessonID).ToList();
                // list of ints of PrerequisiteLessonIDs

                var lessons = await _lessonRepository.GetLessonsByIdsAsync(prerequisiteIds);

                var lessonsDTOs = lessons
                    .Where(lesson => lesson != null)
                    .Select(lesson => new lessonPrerequisiteDTO
                    {
                        LessonTitle = lesson.LessonTitle,
                        lessonOrder = lesson.LessonOrder,
                    })
                    .ToList();

                return Ok(lessonsDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occcured while processing your requiest.");
            }
        }

        // 8. Mark Lesson as Completed
        [HttpPost("lessons/{lessonId}/complete")]
        public async Task<IActionResult> LessonCompleted(/*string userId, */int lessonId)
        {
            var userId = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authonticated.");
            }
            var existingCompletion = await _lessonRepository.existingCompletion(userId, lessonId);
            // get Completed lessons for student from the DB
            if (existingCompletion != null)
            {
                return Conflict("Lesson already marked as completed.");
            }
            var lesson = _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
            {
                return Conflict("No lessons found.");
            }
            var lessonCompletion = new Lesson_Completions
            {
                UserID = userId,
                LessonID = lessonId,
                CompletionDate = DateTime.UtcNow,
            };

           await _genericlessonCompletions.AddAsync(lessonCompletion);

            return CreatedAtAction(
               nameof(GetById),
               new { id = lessonId },
               lessonCompletion
               );
        }

        // 9. Check Prerequisite Completion Before Entering Lesson --> not nessecary.

        // 10. Get Lessons Ordered by Prerequisite Completion
        [HttpGet("sublessons/{subLessonId}/lessons/ordered")]
        public async Task<IActionResult> GetLessonsOrdereByPrereqCompletion(/*string userId,*/ int subLessonId)
        {
            var lessons = await _lessonRepository.GetLessonsBySubLessonIdAsync(subLessonId);

            var userId = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authonticated.");
            }

            var completedLessons = await _lessonRepository.GetLessonsOrderedByPrerequisiteCompletion(subLessonId, userId);
            if (completedLessons == null || !completedLessons.Any())
            {
                return NotFound("No lessons completed for the given sublesson.");
            }

            List<lessonPrerequisiteDTO> lessonsDTOs = new List<lessonPrerequisiteDTO>();
            foreach (var lesson in completedLessons)
            {
                lessonsDTOs.Add(new lessonPrerequisiteDTO
                {
                    LessonTitle = lesson.LessonTitle,
                    lessonOrder = lesson.LessonOrder
                });
            }

            return Ok(lessonsDTOs);
        }

        // 11. Get All Lessons by crsId
        [HttpGet("courses/{crsId}/lessons")]
        public async Task<IActionResult> GetAllByCrsId(int crsId)
        {
            try
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
                if (course == null)
                {
                    return NotFound($"Course with ID: {crsId} not found");
                }
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
            }

            // this function return all lossons by crsId and return the commetns on the course and crsName 
            // then map all on lessonDTO
            catch (Exception ex)
            {
                return StatusCode(500, $"an error occurred while processing your request, {ex.Message}");
            }
        }

        // may be i Will do -->
        // Optionally, trigger unlocking of prerequisite lessons (e.g., checking prerequisites)
        // await UnlockPrerequisitesForStudent(userId, lessonId);
        // private async Task UnlockPrerequisitesForStudent(string userId, int lessonId)

        // Get All Lessons by CrsId with LessonDTO --> return comments in every lesson (:


    }
}
