using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EducationalSystem.DAL.Models;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor to inject UserManager
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/user/getall
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve all users from the UserManager
            var users = _userManager.Users.ToList();

            // If you want to return the full information of each user
            // (including all properties), you can return the users directly like this:
            return Ok(users);
        }
    }
}
