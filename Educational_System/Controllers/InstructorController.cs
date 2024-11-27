using Microsoft.AspNetCore.Identity;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Educational_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IGenericRepository<Instructors> _instructorsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorController(IGenericRepository<Instructors> instructorsRepository, UserManager<ApplicationUser> userManager)
        {
            _instructorsRepository = instructorsRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var instructorsQuery = await _instructorsRepository.GetAll();
            var instructors = await instructorsQuery
                .Include(i => i.Course_Instructors)
                    .ThenInclude(ci => ci.Courses)
                .Include(i => i.Specializations)
                .ToListAsync();

            var instructorInfo = new List<InstructorInfo>();
            foreach (var instructor in instructors)
            {
                var user = await _userManager.FindByIdAsync(instructor.UserID);

                instructorInfo.Add(new InstructorInfo
                {
                    InstructorName = user?.Name,
                    SpecializationsName = instructor.Specializations?.SpecializationName,
                    BIO = instructor.BIO,
                    Courses = instructor.Course_Instructors?.Select(ci => ci.Courses).ToList()
                });
            }

            return Ok(instructorInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await _instructorsRepository.GetByIdAsync(id);

            if (instructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            var user = await _userManager.FindByIdAsync(instructor.UserID);

            if (user == null)
            {
                return NotFound($"User associated with Instructor ID {id} not found.");
            }

            var instructorInfo = new GetInstructorByID
            {
                InstructorName = user.Name,
                SpecializationsName = instructor.Specializations?.SpecializationName,
                BIO = instructor.BIO,
                ProfileImageURL = user.ProfileImageURL,
                PhoneNumber = instructor.PhoneNumber,
                Courses = instructor.Course_Instructors?.Select(ci => ci.Courses).ToList()
            };

            return Ok(instructorInfo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletebyId(int id)
        {
            var instructor = await _instructorsRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return Ok("No instructor with the given ID.");
            }
            await _instructorsRepository.DeleteAsync(instructor);
            return Ok("Instructor deleted successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GetInstructorByID updatedInstructor)
        {
            if (updatedInstructor == null || id <= 0)
            {
                return BadRequest("Invalid instructor data or ID.");
            }

            var existingInstructor = await _instructorsRepository.GetByIdAsync(id);
            if (existingInstructor == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            existingInstructor.BIO = updatedInstructor.BIO;
            existingInstructor.PhoneNumber = updatedInstructor.PhoneNumber;

            // Handle Specializations lookup
            if (!string.IsNullOrEmpty(updatedInstructor.SpecializationsName))
            {
                // Await the Task to get the IQueryable<Instructors> first
                var instructors = await _instructorsRepository.GetAll();

                // Now apply the Where clause to filter by SpecializationName
                var specialization = await instructors
                    .Where(i => i.Specializations.SpecializationName == updatedInstructor.SpecializationsName)
                    .Select(i => i.Specializations)
                    .FirstOrDefaultAsync();


                existingInstructor.SpecializationsID = specialization?.ID;
            }

            // Handle User lookup and update
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Name == updatedInstructor.InstructorName);
            if (user != null)
            {
                existingInstructor.UserID = user.Id;
            }

            await _instructorsRepository.UpdateAsync(existingInstructor);

            return Ok($"Instructor with ID {id} updated successfully.");
        }
    }
}
