using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Repositories.Repositories;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        IGenericRepository<Instructors> InstructorsRepository;
        public InstructorController(IGenericRepository<Instructors> instructorsRepository)
        {
            InstructorsRepository = instructorsRepository;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(InstructorsRepository.GetAll());
        }
    }
}
