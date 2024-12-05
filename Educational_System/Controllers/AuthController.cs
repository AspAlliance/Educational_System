using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(RegisterBS register)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(register);
                IdentityResult result = await userManager.CreateAsync(applicationUser, register.Password);

                if (result.Succeeded)
                {
                    return Ok("Create");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginBS userfromreq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userfromdb = await userManager.FindByNameAsync(userfromreq.UserName);
                if (userfromdb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userfromdb, userfromreq.Password);
                    if (found)
                    {
                        List<Claim> userclaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userfromdb.Id),
                    new Claim(ClaimTypes.Name, userfromdb.UserName)
                };

                        var UserRole = await userManager.GetRolesAsync(userfromdb);
                        foreach (var rolename in UserRole)
                        {
                            userclaims.Add(new Claim(ClaimTypes.Role, rolename));
                        }

                        var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asd!@#$!#ii2@@123okpekrg%%$&(fgq35uRRTT823sdg"));
                        SigningCredentials signingCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: "http://localhost:4200/",
                            issuer: "http://localhost:5088/",
                            expires: DateTime.Now.AddHours(1),
                            claims: userclaims,
                            signingCredentials: signingCred
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(1)
                        });
                    }
                }

                ModelState.AddModelError("username", "username or password is wrong");
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(SignOut))]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok("Signed out succussfully");
        }

    }
}
