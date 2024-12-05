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

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;
        public InstructorController(IInstructorRepository instructorRepository,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {

            _instructorRepository = instructorRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
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
    }
}
