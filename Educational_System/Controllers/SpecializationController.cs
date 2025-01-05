using AutoMapper;
using Educational_System.Dto.Specialization;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Specification;
using EducationalSystem.BLL.Specification.Specs;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly IGenericRepository<Specializations> _repository;
        private readonly IMapper _mapper;

        public SpecializationController(IGenericRepository<Specializations> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spec = new SpecializationSpecification();
            var specializations = await _repository.GetAllWithSpec(spec);
            var specializationsInfo = _mapper.Map<List<getSpecializationDto>>(specializations);
            return Ok(specializationsInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var spec = new SpecializationSpecification();
            var specialization = await _repository.GetByIdWithSpecAsync(id,spec);
            var specializationInfo = _mapper.Map<getSpecializationDto>(specialization);
            if (specializationInfo == null)
                return NotFound("Specialization not found.");

            return Ok(specializationInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization([FromBody] postSpecializationDto specializationDto)
        {

            var mappedSpecialization = _mapper.Map<postSpecializationDto, Specializations>(specializationDto);

            if (mappedSpecialization == null)
                return BadRequest("Specialization is null.");

            await _repository.AddAsync(mappedSpecialization);
            return CreatedAtAction(nameof(GetById), new { id = mappedSpecialization.ID }, mappedSpecialization);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, [FromBody] postSpecializationDto specializationDto)
        {
            if (specializationDto == null)
                return BadRequest("Specialization data is null.");

            var existingSpecialization = await _repository.GetByIdAsync(id);
            if (existingSpecialization == null)
                return NotFound("Specialization not found.");

            _mapper.Map(specializationDto, existingSpecialization);

            await _repository.UpdateAsync(existingSpecialization);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var specialization = await _repository.GetByIdAsync(id);
            if (specialization == null)
                return NotFound("Specialization not found.");

            await _repository.DeleteAsync(specialization);
            return NoContent();
        }
    }
}
