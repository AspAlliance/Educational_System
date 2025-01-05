using AutoMapper;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Educational_System.Dto.Choices;
namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly IGenericRepository<Choices> _repository;
        private readonly IMapper _mapper;

        public ChoicesController(IGenericRepository<Choices> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Task 1: Add a New Choice
        [HttpPost]
        public async Task<IActionResult> AddChoice([FromBody] PostChoicesDto choiceDto)
        {
            if (choiceDto == null)
                return BadRequest("Choice data is null.");

            var mappedChoice = _mapper.Map<Choices>(choiceDto);

            if (mappedChoice == null)
                return BadRequest("Mapping failed.");

            await _repository.AddAsync(mappedChoice);
            return CreatedAtAction(nameof(GetChoiceById), new { choiceId = mappedChoice.ID }, mappedChoice);
        }

        // Task 2: Update an Existing Choice
        [HttpPut("{choiceId}")]
        public async Task<IActionResult> UpdateChoice(int choiceId, [FromBody] PostChoicesDto choiceDto)
        {
            if (choiceDto == null)
                return BadRequest("Choice data is null.");

            var existingChoice = await _repository.GetByIdAsync(choiceId);
            if (existingChoice == null)
                return NotFound("Choice not found.");

            _mapper.Map(choiceDto, existingChoice);

            await _repository.UpdateAsync(existingChoice);
            return NoContent();
        }

        // Task 3: Delete a Choice
        [HttpDelete("{choiceId}")]
        public async Task<IActionResult> DeleteChoice(int choiceId)
        {
            var choice = await _repository.GetByIdAsync(choiceId);
            if (choice == null)
                return NotFound("Choice not found.");

            await _repository.DeleteAsync(choice);
            return NoContent();
        }

        // Task 4: Get Choice by ID
        [HttpGet("{choiceId}")]
        public async Task<IActionResult> GetChoiceById(int choiceId)
        {
            var choice = await _repository.GetByIdAsync(choiceId);
            if (choice == null)
                return NotFound("Choice not found.");

            var choiceDto = _mapper.Map<GetChoicesDto>(choice);
            return Ok(choiceDto);
        }

        // Task 5: Get All Choices for a Question
        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetChoicesByQuestionId(int questionId)
        {
            var choices = await _repository.GetAll();
            var choicesForQuestion = choices.Where(c => c.QuestionID == questionId).ToList();

            var choicesDto = _mapper.Map<List<GetChoicesDto>>(choicesForQuestion);
            return Ok(choicesDto);
        }

        // Task 6: Set the Correct Choice for a Question
        [HttpPut("{choiceId}/set-correct")]
        public async Task<IActionResult> SetCorrectChoice(int choiceId)
        {
            var choice = await _repository.GetByIdAsync(choiceId);
            if (choice == null)
                return NotFound("Choice not found.");

            // Get all choices for the same question
            var allChoices = await _repository.GetAll();
            var choicesForQuestion = allChoices.Where(c => c.QuestionID == choice.QuestionID).ToList();

            // Ensure only one choice is marked as correct
            foreach (var c in choicesForQuestion)
            {
                c.IsCorrect = (c.ID == choiceId) ? 1 : 0;
                await _repository.UpdateAsync(c);
            }

            return NoContent();
        }

        // Task 7: Check if a Choice is Correct
        [HttpGet("{choiceId}/is-correct")]
        public async Task<IActionResult> IsChoiceCorrect(int choiceId)
        {
            var choice = await _repository.GetByIdAsync(choiceId);
            if (choice == null)
                return NotFound("Choice not found.");

            return Ok(new { IsCorrect = choice.IsCorrect == 1 });
        }
    }
}