using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Educational_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextSubmissionController : ControllerBase
    {
        private readonly IGenericRepository<TextSubmissions> _genericRepository;
        private readonly IMapper _mapper;
        public TextSubmissionController(IGenericRepository<TextSubmissions> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            
        }

        // 1. Submit Text
        [HttpPost]
        public async Task<IActionResult> SubmitText(TextSubmissionDTO textSubmissionDTO)
        {
            if (textSubmissionDTO == null)
            {
                return BadRequest("Text submission cannot be null!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newTextSubmission = new TextSubmissions
                {
                    AssessmentID = textSubmissionDTO.AssessmentID,
                    UserID = textSubmissionDTO.UserID,
                    Grade = textSubmissionDTO.Grade,
                    SubmittedDate = DateTime.UtcNow,
                    ResponseText = textSubmissionDTO.ResponseText,
                    Feedback = textSubmissionDTO.Feedback,
                };
                await _genericRepository.AddAsync(newTextSubmission);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while adding new text submission");
            }
            return Ok(textSubmissionDTO);
        }

        // 2. Update Text Submission
        [HttpPut("{submissionId}")]
        public async Task<IActionResult> UpdateText(int submissionId, [FromBody] TextSubmissionDTO textSubmissionDTO)
        {
            var textSubmission = await _genericRepository.GetByIdAsync(submissionId);
            if (textSubmission == null)
            {
                return BadRequest("not found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                textSubmission.AssessmentID = textSubmissionDTO.AssessmentID;
                textSubmission.UserID = textSubmissionDTO.UserID;
                textSubmission.ResponseText = textSubmissionDTO.ResponseText;
                textSubmission.Grade = textSubmissionDTO.Grade;
                textSubmission.SubmittedDate = DateTime.UtcNow;
                textSubmission.Feedback = textSubmissionDTO.Feedback;
            }
            catch
            {
                return StatusCode(500, "An error occured while updating the text submission");
            }
            return Ok(textSubmissionDTO);
        }

        // 3. Delete Text Submission
        [HttpDelete("{submissionId}")]
        public async Task<IActionResult> Delete(int submissionId)
        {
            var textSubmission = await _genericRepository.GetByIdAsync(submissionId);
            if (textSubmission == null)
            {
                return BadRequest("not found");
            }

            try
            {
                await _genericRepository.DeleteAsync(textSubmission);
            }
            catch
            {
                return StatusCode(500, "An error occured while Deleting the text submission");
            }
            return NoContent();
        }

        // 4. Get Text Submission by ID
        [HttpGet("/{submissionId}")]
        public async Task<IActionResult> GetById(int submissionId)
        {
            var textSubmission = await _genericRepository.GetByIdAsync(submissionId);
            if (textSubmission == null)
            {
                return BadRequest("not found");
            }
            var newMappedSubmission = _mapper.Map<TextSubmissionDTO>(textSubmission);

            return Ok(newMappedSubmission);
        }

        [HttpPost("{submissionId}/grade")]
        public async Task<IActionResult> GradeTextSubmission(int submissionId, int grade)
        {
            var submission = await _genericRepository.GetByIdAsync(submissionId);
            if (submission == null)
                return BadRequest("not found");

            try
            {
                submission.Grade = grade;
                await _genericRepository.UpdateAsync(submission);
            }
            catch
            {
                return StatusCode(500, "An error occured while Adding Grade");
            }
            return Ok(new { message = "Grade submitted successfully.", submission });
        }
    }
}
