using Microsoft.AspNetCore.Identity; // For UserManager
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
        private readonly IInstructorRepository instructorRepository;
        public InstructorController(IGenericRepository<Instructors> instructorsRepository,IInstructorRepository instructorRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.instructorRepository = instructorRepository;
            _instructorsRepository = instructorsRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Fetch the instructors along with related entities
            var instructorsQuery = await _instructorsRepository.GetAll();
            var instructors = await instructorsQuery
                .Include(i => i.Course_Instructors)
                    .ThenInclude(ci => ci.Courses)
                .Include(i => i.Specializations)
                .ToListAsync();

            // Map to InstructorInfo ViewModel
            var instructorInfo = new List<InstructorInfo>();
            foreach (var instructor in instructors)
            {
                // Fetch the user information using UserManager
                var user = await _userManager.FindByIdAsync(instructor.UserID);

                // Add the instructor data to the ViewModel
                instructorInfo.Add(new InstructorInfo
                {
                    InstructorName = user?.Name, // Assuming 'Name' is a property in ApplicationUser
                    SpecializationsName = instructor.Specializations?.SpecializationName,
                    BIO = instructor.BIO,
                    Courses = instructor.Course_Instructors?.Select(ci => ci.Courses).ToList()
                });
            }

            return Ok(instructorInfo);
        }
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUsersOfInstructor(int id)
        {
            var users = await instructorRepository.GeInstructorUsersAsync(id);
            return Ok(users);
        }
    }
}
