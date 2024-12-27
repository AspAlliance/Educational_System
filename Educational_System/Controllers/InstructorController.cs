using Microsoft.AspNetCore.Identity; // For UserManager
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Educational_System.Dto;
using NuGet.Protocol;
using Newtonsoft.Json;
using EducationalSystem.BLL.Specification.Specs;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using InstructorWUserWSpecial = EducationalSystem.BLL.Repositories.Interfaces.InstructorWUserWSpecial;
using System.IdentityModel.Tokens.Jwt;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InstructorController> logger;
        private readonly ISpecializationsRepository _specializationsRepository;

        public InstructorController(IInstructorRepository instructorRepository,
            UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<InstructorController> _logger, ISpecializationsRepository specializationsRepository)
        {
            _instructorRepository = instructorRepository;
            _userManager = userManager;
            _mapper = mapper;
            logger = _logger;
            _specializationsRepository = specializationsRepository;
        }


        [HttpGet] // Should understand !!??
        public async Task<IActionResult> GetAll()
        {
            // Define the filtering criteria with Specification.
            var instructorSpec = new InstructorSpecification();
            //get all instructors based on the defined specification.
            var instructors = await _instructorRepository.GetAllWithSpec(instructorSpec);

            var instructorInfo = _mapper.Map<List<InstructorsDto>>(instructors);

            // The following block sets up custom JSON settings, but it's not needed here.
            // ASP.NET Core already handles JSON serialization by default, so we can simplify the code.
            var jsonSettings = new JsonSerializerSettings
            {
                // Prevents circular references in JSON without infite loop
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            // Return the data as JSON with manual serialization. This can be simplified.
            return Ok(JsonConvert.SerializeObject(instructorInfo, jsonSettings));
            // A simpler and better approach is to just return the DTO directly, as ASP.NET Core 
            // takes care of serializing the response for you.
            //return Ok(instructorInfo);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUsersOfInstructor(int id)
        {
            var users = await _instructorRepository.GeInstructorUsersAsync(id);
            return Ok(users);
        }

        // Get instructor Details By Id
        [HttpGet("{id}")] // Done 
        public async Task<IActionResult> GetInstructorById(int id)
        {   
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
                return NotFound($"Instructor with ID {id} not found.");

            ApplicationUser userFromDb = await _instructorRepository.GetInstructorUserByIdAsync(id);
            if (userFromDb == null)
                return NotFound($"user with instructor id {id} not found");

            Specializations userSpecialization = await _specializationsRepository.GetInstructorsBySpecializationIdAsync(instructor.SpecializationsID);
            if (userSpecialization == null)
                return NotFound($"Specialization for instructor ID {id} not found.");

            InstructorWUserWSpecial instructoDTO = new InstructorWUserWSpecial
            {
                Insname = userFromDb.Name,
                InsphoneNumber = instructor.PhoneNumber,
                InsNationalCardImg = instructor.NationalCardImageURL,
                BIO = instructor.BIO,
                specialName = userSpecialization.SpecializationName,
                CV_PDF_URL = instructor.CV_PDF_URL,
                ProfileImageURL = userFromDb.ProfileImageURL,
            };
            return Ok(instructoDTO);
        }

        // Check it ??!! 
        // Add New Instructor
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Instructors instructorFromRequest)
        {
            
            if (instructorFromRequest == null)
            {
                logger.LogWarning("Instructor data is missing.");
                return BadRequest("Instructor data is required.");
            }
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Model state is invalid for the provided instructor.");
                return BadRequest(ModelState); // Return validation errors
            }
            try
            {
                await _instructorRepository.AddAsync(instructorFromRequest);
                logger.LogInformation($"Instructor with ID {instructorFromRequest.ID} added successfully.", instructorFromRequest.ID);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while adding instructor with ID {instructorFromRequest.ID}.",
                    instructorFromRequest.ID);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            //return Ok(instructorFromRequest);
            // Return 201 Created 
            return CreatedAtAction(
               nameof(GetInstructorById), // Action that retrieves the created resource
               new { id = instructorFromRequest.ID }, // Route values
               instructorFromRequest // The response body
                );
        }

        // Edit page
        [HttpGet("{id}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            Instructors instructorFromDb = await _instructorRepository.GetByIdAsync(id);
            if (instructorFromDb == null)
                return NotFound($"instructor with id {id} not found");

            ApplicationUser user = await _instructorRepository.GetInstructorUserByIdAsync(id);
            if (user == null)
                return NotFound("user not found");

           IQueryable<Specializations> specializations = await _specializationsRepository.GetAll();
            if (specializations == null)
                return NotFound("no specialization");

            InstructorWSpeciListDTO instructorDTO = new InstructorWSpeciListDTO
            {
                Insname = user.Name,
                InsphoneNumber = instructorFromDb.PhoneNumber,
                InsNationalCardImg = instructorFromDb.NationalCardImageURL,
                BIO = instructorFromDb.BIO,
                CV_PDF_URL = instructorFromDb.CV_PDF_URL,
                ProfileImageURL = user.ProfileImageURL,
                specializations = specializations
            };
            return Ok(instructorDTO);
        }

        // Edit On instructor
        [HttpPut("{id}")] // Done
        public async Task<IActionResult> SaveEdit(int id, [FromBody] InstructorWUserWSpecial instructorFromReq)
        {
            if (instructorFromReq == null)
                return BadRequest("Invalid data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructorFromDb = await _instructorRepository.GetByIdAsync(id);
            if (instructorFromDb == null)
                return NotFound($"Instructor with id {id} not found");

            Specializations specialization = await _specializationsRepository.GetInstructorsBySpecializationIdAsync(instructorFromDb.SpecializationsID);
            // Get specilaztion based on instructor id

            if (specialization == null)
                return NotFound($"specialization with id {specialization.ID} not found");

            ApplicationUser userDataFromDb = await _instructorRepository.GetInstructorUserByIdAsync(id);

            if (userDataFromDb == null)
                return NotFound($"user with id {userDataFromDb.Id} not found");

            try
            {
                // Update user data
                userDataFromDb.Name = instructorFromReq.Insname; 
                // Update instructor data
                userDataFromDb.ProfileImageURL = instructorFromReq.ProfileImageURL;
                instructorFromDb.PhoneNumber = instructorFromReq.InsphoneNumber;
                instructorFromDb.BIO = instructorFromReq.BIO;
                instructorFromDb.CV_PDF_URL = instructorFromReq.CV_PDF_URL;
                instructorFromDb.NationalCardImageURL = instructorFromReq.InsNationalCardImg;
                // Update specialization
                instructorFromDb.SpecializationsID = specialization.ID; // Map specialization
                // specialization.SpecializationName = instructorFromReq.specialName;

                await _instructorRepository.UpdateAsync(instructorFromDb);
                await _specializationsRepository.UpdateAsync(specialization);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error updating instructor with ID {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
            return NoContent(); // 204 No Content: Successful update without returning data
        }

        [HttpDelete]  // Done
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var instructor = await _instructorRepository.GetByIdAsync(id);
                if (instructor == null) return NotFound($"instrctor with id {id} not found");

                await _instructorRepository.DeleteAsync(instructor);
                return NoContent(); // 204 No Content: Successful Deleting data
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, $"An error occurred while deleting instructor with ID {id}.");
                return StatusCode(500, $"Internal server error, {ex.Message}");
            }
        }


    }
}
