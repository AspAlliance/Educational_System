using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Educational_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubricsController : ControllerBase
    {
        private readonly IGenericRepository<Rubrics> _genericRepository;
        private readonly IGenericRepository<Assessments> _assessmentRepository;

        public RubricsController(IGenericRepository<Rubrics> genericRepository,
            IGenericRepository<Assessments> assessmentRepository)
        {
            _genericRepository = genericRepository;
            _assessmentRepository = assessmentRepository;
            
        }

        [HttpPost]
        public async Task<IActionResult> Add(RubricsDTO rubricsDTO)
        {
            if (rubricsDTO == null)
            {
                return BadRequest("Data Cannot be Null");
            }

            // Verify that the related assessment exists.
            var assesment = await _assessmentRepository.GetByIdAsync(rubricsDTO.AssessmentID);
            if (assesment == null)
                return BadRequest($"there is no assesment with id {rubricsDTO.AssessmentID}");

            var rubrics = await _genericRepository.GetAll();

            // check if assesmnt id is already exect in DB 
            foreach (var rubric in rubrics)
            {
                if (rubric.ID == rubricsDTO.AssessmentID)
                {
                    return Conflict("Cannot adding rubric for assesment already token. ");
                }
            }
            /*
                // Instead of retrieving all rubrics, query for a rubric with the same AssessmentID.
            var existingRubric = await _genericRepository.FindAsync(rubric => rubric.AssessmentID == rubricsDTO.AssessmentID);
            if (existingRubric != null)
            {
                return Conflict("A rubric for this assessment already exists.");
            }
             */
            try
            {
                var newRubric = new Rubrics
                {
                    AssessmentID = rubricsDTO.AssessmentID,
                    Criterion = rubricsDTO.Criterion,
                    MaxPoints = rubricsDTO.MaxPoints,
                };

                await _genericRepository.AddAsync(newRubric);
            }
            catch
            {
                return StatusCode(500, "An error occured while adding a rubric");
            }
            return Ok
                ( new
                {
                    message = "Rubric added successfully",
                    rubricsDTO
                });
        }


        [HttpPut("{rubricId}")]
        public async Task<IActionResult> Update(int rubricId, [FromBody] RubricsDTO rubricsDTO)
        {
            if (rubricsDTO == null)
            {
                return BadRequest("Data cannot be null.");
            }

            // Retrieve the rubric from the database
            var existingRubric = await _genericRepository.GetByIdAsync(rubricId);
            if (existingRubric == null)
            {
                return NotFound($"Rubric with ID {rubricId} not found.");
            }

            // Update the rubric details
            existingRubric.Criterion = rubricsDTO.Criterion;
            existingRubric.MaxPoints = rubricsDTO.MaxPoints;

            try
            {
                await _genericRepository.UpdateAsync(existingRubric);
            }
            catch (Exception ex)
            {
                // Log the exception if logging is implemented
                return StatusCode(500, "An error occurred while updating the rubric.");
            }

            return Ok(new
            {
                message = "Rubric updated successfully.",
                rubricsDTO
            });
        }

        [HttpDelete("{rubricId}")]
        public async Task<IActionResult> Delete(int rubricId)
        {
            // Retrieve the rubric from the database
            var existingRubric = await _genericRepository.GetByIdAsync(rubricId);
            if (existingRubric == null)
            {
                return NotFound($"Rubric with ID {rubricId} not found.");
            }

            try
            {
                await _genericRepository.DeleteAsync(existingRubric);
            }
            catch (Exception ex)
            {
                // Log the exception if logging is implemented
                return StatusCode(500, "An error occurred while deleting the rubric.");
            }

            return Ok(new { message = "Rubric deleted successfully." });
        }


        //[HttpGet("assessment/{assessmentId}")]
        //public async Task<IActionResult> GetByAssessment(int assessmentId)
        //{
            
        //}


    }
}
