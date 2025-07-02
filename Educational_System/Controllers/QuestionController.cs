using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IGenericRepository<Questions> _repository;
        private readonly IGenericRepository<Assessments> _AssessmentsRepository;
        private readonly IGenericRepository<QuestionType> _QuestionTyperepository;
        private readonly IGenericRepository<Choices> _Choicesrepository;
        private readonly IQuestionRepository _IQuestionRepository;
        public QuestionController(IGenericRepository<Questions> repository,
            IGenericRepository<QuestionType> questionTyperepository,
            IGenericRepository<Choices> Choicesrepository, 
            IGenericRepository<Assessments> AssessmentsRepository, IQuestionRepository IQuestionRepository)
        {
            _repository = repository;
            _QuestionTyperepository = questionTyperepository;
            _Choicesrepository = Choicesrepository;
            _AssessmentsRepository = AssessmentsRepository;
            _IQuestionRepository = IQuestionRepository;
        }

        // 1. Add Question to Assessment (not Done !!)
        [HttpPost("assessments/{assessmentId}/questions")]
        public async Task<IActionResult> AddQuestion(int assessmentId, [FromBody] AddQuestionDTO QuestionDTO)
        {
            var assesment = await _AssessmentsRepository.GetByIdAsync(assessmentId);
            if (assesment == null)
            {
                return NotFound("assesment not found");
            }

            if (QuestionDTO == null || string.IsNullOrEmpty(QuestionDTO.QuestionText) || QuestionDTO.Points <= 0)
            {
                return BadRequest("Invalid question data.");
            }

            var questionType = await _QuestionTyperepository.GetByIdAsync(QuestionDTO.QuestionTypeID);
            if (questionType == null)
            {
                return BadRequest("Invalid question type.");
            }

            var newQuestion = new Questions
            {
                AssessmentID = assessmentId,
                QuestionText = QuestionDTO.QuestionText,
                QuestionTypeID = QuestionDTO.QuestionTypeID,
                Points = QuestionDTO.Points,
            };
            try
            {
                await _repository.AddAsync(newQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while saving Question.");   
            }

            if (QuestionDTO.QuestionTypeName.Equals("MCQ", StringComparison.OrdinalIgnoreCase))
            {

                if (QuestionDTO.Choices == null || QuestionDTO.Choices.Count < 2)
                {
                    return BadRequest("MCQ questions must have at least two choices.");
                }

                var answerChoise = new Choices
                {
                    ChoiceText = QuestionDTO.ChoiceText,
                    QuestionID = newQuestion.ID,
                    IsCorrect = 0
                };
                try
                {
                    await _Choicesrepository.AddAsync(answerChoise);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "an error occured while saving Question.");
                }

            }
            else if (QuestionDTO.QuestionTypeName.Equals("Essay", StringComparison.OrdinalIgnoreCase))
            {
                
            }
            else
            {
                return BadRequest("Unsupported question type.");
            }
            return Ok(QuestionDTO);
        }


        // 2. Get Questions by Assessment 
        [HttpGet("assessments/{assessmentId}/questions")]
        public async Task<IActionResult> GetQuestionsbyAssessment (int assessmentId)
        {
            var assesment = await _AssessmentsRepository.GetByIdAsync(assessmentId);
            if (assesment == null)
                return NotFound("no assesment found");

            var questions = await _IQuestionRepository.GetAllByAssesmentId(assessmentId);
            if (!questions.Any() || questions == null)
                return NotFound("no questions for this assesment");

            
            var questionDTO = new List<QuestionAssessmentDTO>();
            foreach (var question in questions)
            {
                questionDTO.Add(new QuestionAssessmentDTO
                {
                    Points = question.Points,
                    QuestionText = question.QuestionText,
                    QuestionId = question.ID,
                });
            }
            
            return Ok(questionDTO);
        }

        // 3. Get Specific Question
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            if (questionId <= 0)
            {
                return BadRequest("not valid id");
            }
            var quesion = await _IQuestionRepository.GetQuestionWithAssesmentAndQuesType(questionId);
            if (quesion ==null)
            {
                return NotFound($"No question found with ID {questionId}.");
            }
            
            var questionDTO = new QuestionDTO
            {
                Points = quesion.Points,
                QuestionText = quesion.QuestionText,
                AssessmentType = quesion.Assessments.AssessmentType,
                QuestionTypeName = quesion.QuestionType.QuestionTypeName,
            };
            return Ok(questionDTO);
        }

        // 4. Update Question
        [HttpPut(nameof(UpdateQuestion))]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionAssessmentDTO questionDTO)
        {
            if (questionId <= 0)
            {
                return BadRequest("Question ID must be greater than zero");
            }
            if (questionDTO == null)
            {
                return BadRequest("question cannot be Empty");
            }

            if (string.IsNullOrWhiteSpace(questionDTO.QuestionText))
            {
                return BadRequest("Question text cannot be empty.");
            }
            if (questionDTO.Points < 0)
            {
                return BadRequest("Points must be a non-negative value.");
            }

            var question = await _repository.GetByIdAsync(questionId);
            if (question == null)
            {
                return BadRequest($"Question {questionId} was not found");

            }

            question.QuestionText = questionDTO.QuestionText;
            question.Points = questionDTO.Points;
            try
            {
                await _repository.UpdateAsync(question);
            }
            catch
            {
                return StatusCode(500, "an error occured while updating the question.");
            }
            return Ok(new
            {
                message = "Question updated successfully.",
                updatedQuestion = questionDTO
            });
        }

        // 5. Delete Question
        [HttpDelete(nameof(DeleteQuestion))]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            if (questionId <= 0)
                return BadRequest("Question ID must be greater than zero.");

            var question = await _repository.GetByIdAsync(questionId);
            if (question == null)
            {
                return NotFound($"Question with ID {questionId} was not found.");
            }

            try
            {
                await _repository.DeleteAsync(question);
            }
            catch
            {
                return StatusCode(500, "An error occured while Deleting the Question. ");
            }
            return Ok(new
            {
                message = "Question Deleted successfully.",
            });
        }



    }
}
