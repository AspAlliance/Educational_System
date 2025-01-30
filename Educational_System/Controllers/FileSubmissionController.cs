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
    public class FileSubmissionController : ControllerBase
    {
        private readonly IGenericRepository<FileSubmissions> _genericRepository;
        private readonly IGenericRepository<Assessments> _assesmentRepository;
        private readonly IMapper _mapper;

        public FileSubmissionController(IGenericRepository<FileSubmissions> genericRepository, 
            IGenericRepository<Assessments> assesmentRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _assesmentRepository = assesmentRepository;
            _mapper = mapper;
           // _environment = environment;
        }

        // 1. Submit a File ??? not completed
        [HttpPost(nameof(SubmitFile))]
        public async Task<IActionResult> SubmitFile([FromBody] FileSubmissionDTO fileSubmissionDTO)
        {
            if (fileSubmissionDTO == null || fileSubmissionDTO.File == null)
            {
                return BadRequest("invalid file submission");
            }

            // Create file submission entry
            var fileSubmission = new FileSubmissions
            {
                AssessmentID = fileSubmissionDTO.AssessmentID,
                UserID = fileSubmissionDTO.UserID,
                SubmittedDate = DateTime.UtcNow,
                FilePath = fileSubmissionDTO.File,
                Grade = fileSubmissionDTO.Grade,
                Feedback = string.Empty
            };

            try
            {
                await _genericRepository.AddAsync(fileSubmission);
            }
            catch
            {
                return StatusCode(500, "An error occured while adding the file");
            }
            return Ok(new
            {
                message = "File submitted successfully.",
                fileSubmission.FilePath
            });
        }

        // 2. Update File Submission

        [HttpPut("{submissionId}")]
        public async Task<IActionResult> UpdateFile(int submissionId, [FromBody]FileSubmissionDTO fileSubmissionDTO)
        {
            var fileSubmision = await _genericRepository.GetByIdAsync(submissionId);
            if (fileSubmision == null)
                return BadRequest("not found");

            try
            {
                fileSubmision.FilePath = fileSubmissionDTO.File;
                fileSubmision.AssessmentID = fileSubmissionDTO.AssessmentID;
                fileSubmision.UserID = fileSubmissionDTO.UserID;
                fileSubmision.Grade = fileSubmissionDTO.Grade;

                await _genericRepository.UpdateAsync(fileSubmision);
            }
            catch
            {
                return StatusCode(500, "An error occured while updating the file");
            }
            return Ok(fileSubmissionDTO);
        }

        // 3. Delete File Submission
        [HttpDelete("{submissionId}")]
        public async Task<IActionResult> Delete (int submissionId)
        {
            var fileSubmision = await _genericRepository.GetByIdAsync(submissionId);
            if (fileSubmision == null)
                return BadRequest("not found");

            try
            {
                await _genericRepository.DeleteAsync(fileSubmision);
            }
            catch
            {
                return StatusCode(500, "An error occured while Deleting the file");
            }
            return NoContent();
        }

        // 4 . Get File Submission by ID
        [HttpGet("{submissionId}")]
        public async Task<IActionResult> GetById(int submissionId)
        {
            var fileSubmision = await _genericRepository.GetByIdAsync(submissionId);
            if (fileSubmision == null)
                return BadRequest("not found");
            FileSubmissionDTO fileSubmissionDTO = new FileSubmissionDTO
            {
                AssessmentID = fileSubmision.AssessmentID,
                UserID = fileSubmision.UserID,
                File = fileSubmision.FilePath,
                Grade = fileSubmision.Grade,
            };
            
            return Ok(fileSubmissionDTO);
        }

        // 5. Grade File Submission
        [HttpPost("{submissionId}/grade")]
        public async Task<IActionResult> GradeFile(int submissionId, int grade)
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
