using Educational_System.Dto;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    { 
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            // Materialize the data into a list
            var users = await _userManager.Users.ToListAsync();

            List<UsersInfoDto> UsersInfo = new List<UsersInfoDto>();

            foreach (var user in users)
            {
                var userInfo = new UsersInfoDto
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                UsersInfo.Add(userInfo);
            }

            return Ok(UsersInfo);
        }



    }
}