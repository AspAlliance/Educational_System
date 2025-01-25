using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IGenericRepository<Assessments> _repository;
        private readonly IGenericRepository<Assessment_Results> _assessmentResultsRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMapper _mapper;

        public AssessmentController(IGenericRepository<Assessments> repository 
            , IMapper mapper, IAssessmentRepository assessmentRepository,
            IGenericRepository<Assessment_Results> assessmentResultsRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _assessmentRepository = assessmentRepository;
            _assessmentResultsRepository = assessmentResultsRepository;
        }

        // 1. Create an Assessment 
        [HttpPost]
        public async Task<IActionResult> AddAssessment([FromBody] AssessmentsDto assessmentDto)
        {
            if (assessmentDto == null)
                return BadRequest("Assesmetn is null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mappedassessment = _mapper.Map<AssessmentsDto, Assessments>(assessmentDto);

            if (mappedassessment == null)
                return BadRequest("Assessment is null.");
            
            try
            {
                await _repository.AddAsync(mappedassessment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while saving the assesment");
            }
            return CreatedAtAction(nameof(GetById), new { id = mappedassessment.ID }, assessmentDto);
        }

        // 2. Get All Assessments for a Course 
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetAllbyCrsId(int courseId)
        {
            if (courseId <= 0)
                return BadRequest("Invalid course Id");
            var assesmnts = await _assessmentRepository.GetAllByCrsId(courseId);

            if (assesmnts == null || !assesmnts.Any())
            {
                return NotFound($"no assesments found with course ID {courseId}");
            }

            //_mapper.Map(assesmnts, assessmentsDto);
            var assessmentsDto = _mapper.Map<List<AssessmentsDto>>(assesmnts);

            return Ok(assessmentsDto);
        }

        // 3. Get All Assessments for a Lesson
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetAllbyLessonId(int lessonId)
        {
            if (lessonId <= 0)
                return BadRequest("invalid lesson Id");

            var assesmnts = await _assessmentRepository.GetAllByLessonId(lessonId);
            if (assesmnts == null)
            {
                return NotFound($"no assesments found with lesson ID {lessonId}");
            }

            var assesmentsDTO = _mapper.Map<List<AssessmentsDto>>(assesmnts);
            return Ok(assesmentsDTO);
        }
        
        // 4. Get Assessment Details by ID 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var assessment = await _repository.GetByIdAsync(id);
            if (assessment == null)
                return NotFound("Assessment not found.");

            return Ok(assessment);
        }

        // 5. Delete Assessment 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(int id)
        {
            var assessment = await _repository.GetByIdAsync(id);
            if (assessment == null)
                return NotFound("Assessment not found.");

            await _repository.DeleteAsync(assessment);
            return NoContent();
        }

        // 6. Update Assessment 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssessment(int id, [FromBody] AssessmentsDto assessmentDto)
        {
            if (id <= 0)
                return BadRequest("Invalid assessment ID.");

            if (assessmentDto == null)
                return BadRequest("Assessment data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingAssessment = await _repository.GetByIdAsync(id);
                if (existingAssessment == null)
                  return NotFound("Assessment not found.");

               _mapper.Map(assessmentDto, existingAssessment);
           
                await _repository.UpdateAsync(existingAssessment);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                // if another user updating the assesment in the same time
                return Conflict("The assessment was updated by another user. Please reload and try again.");
            } 
            catch(Exception ex)
            {
                return StatusCode(500, "An error occured while updtaing the assesment. ");
            }
        }
        
        // 7. Get All Assessment Results by User & Course ID 
        [HttpGet("students/{userId}/assessment-results")]
        public async Task<IActionResult> GetAllResultsbyUserId(string userId)
        {
            var results = await _assessmentRepository.GetResultsByStudentIdAsync(userId);
            var coursesEnrollment = await _assessmentRepository.GetCoursesEnrollmentByUserId(userId);

            List<AssessmentResultDto> resultsDTO = new List<AssessmentResultDto>();
            //foreach (var course in coursesEnrollment)
            //{
            //    resultsDTO.Add(new AssessmentResultDto
            //    {
            //        crsId = course.CourseId,
            //        crsName = course.Courses.CourseTitle,
            //    });
            //}
            foreach (var result in results)
            {
                resultsDTO.Add(new AssessmentResultDto
                {
                    StudentName = result.User.Name,
                    Score = result.Score,
                    AttemptDate = result.AttemptDate,

                });
            }
           
            return Ok(resultsDTO);
        }

        // 8. Get Assessment Results by Lesson ID 
        //[HttpGet("/students/{userId}/assessment-results")]
        //public async Task<IActionResult> GetAllResultsbyLessonId

        // 9. Update Assessment Result --> updating the score of assesmetn result only
        [HttpPut("assessment-results/{assessmentResultId}")]
        public async Task<IActionResult> UpdateAssesment(int assessmentResultId,[FromBody] AssessmentResultDto assessmentDto)
        {
            var existingAssesResult = await _assessmentResultsRepository.GetByIdAsync(assessmentResultId);
            
            if (existingAssesResult == null)
            {
                return NotFound($"no assesment results with id {assessmentResultId}");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                existingAssesResult.Score = assessmentDto.Score;
                await _assessmentResultsRepository.UpdateAsync(existingAssesResult);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while updating the assesment result");
            }
        }

        // 10. Delete Assessment Resul  
        [HttpDelete("assessment-results/{assessmentResultId}")] 
        public async Task<IActionResult> DeleteAssessResult(int assessmentResultId)
        {
            var assessmentResult = await _assessmentResultsRepository.GetByIdAsync(assessmentResultId);
            if (assessmentResult == null)
                return NotFound("Assessment not found.");
            try
            {
                await _assessmentResultsRepository.DeleteAsync(assessmentResult);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while Deleting the assesment result");
            }
            return NoContent();
        }

        // 11. Add Assessment Result to User
        [HttpPost("assessment-results")]
        public async Task<IActionResult> AddAssessmentResult(AddAssesmentDTO AssesmentDTO)
        {
            if (AssesmentDTO == null)
                return BadRequest("Assesment result is empty");

            try
            {
                Assessment_Results assessment_Results = new Assessment_Results()
                {
                    Score = AssesmentDTO.Score,
                    UserID = AssesmentDTO.UserId,
                };
                await _assessmentResultsRepository.AddAsync(assessment_Results);
                return CreatedAtAction(nameof(GetById), new { id = assessment_Results.ID }, AssesmentDTO);
            }
            catch
            {
                return StatusCode(500, "an error occured while saving the assesment result");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assessments = await _repository.GetAll();
            return Ok(assessments);
        }


    }
   
}
