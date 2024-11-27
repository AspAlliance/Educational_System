using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IGenericRepository<Assessments> _repository;
        private readonly IMapper _mapper;

        public AssessmentController(IGenericRepository<Assessments> repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var assessments = await _repository.GetAll();
            return Ok(assessments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssessment(int id)
        {
            var assessment = await _repository.GetByIdAsync(id);
            if (assessment == null)
                return NotFound("Assessment not found.");

            return Ok(assessment);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssessment([FromBody] AssessmentsDto assessmentDto)
        {

            var mappedassessment = _mapper.Map<AssessmentsDto, Assessments>(assessmentDto);

            if (mappedassessment == null)
                return BadRequest("Assessment is null.");

            await _repository.AddAsync(mappedassessment);
            return CreatedAtAction(nameof(GetAssessment), new { id = mappedassessment.ID }, mappedassessment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssessment(int id, [FromBody] AssessmentsDto assessmentDto)
        {
            if (assessmentDto == null)
                return BadRequest("Assessment data is null.");

            var existingAssessment = await _repository.GetByIdAsync(id);
            if (existingAssessment == null)
                return NotFound("Assessment not found.");

            _mapper.Map(assessmentDto, existingAssessment);

            await _repository.UpdateAsync(existingAssessment);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(int id)
        {
            var assessment = await _repository.GetByIdAsync(id);
            if (assessment == null)
                return NotFound("Assessment not found.");

            await _repository.DeleteAsync(assessment);
            return NoContent();
        }

    }
   
}
