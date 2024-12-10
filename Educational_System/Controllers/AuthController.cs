using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
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
        private readonly IGenericRepository<Instructors> _repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager; // Add RoleManager

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Instructors> _repository,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager // Inject RoleManager
        )
        {
            _mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._repository = _repository;
            this.roleManager = roleManager; // Assign injected RoleManager
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

        [HttpPost(nameof(RegisterAsInstructor))]
        public async Task<IActionResult> RegisterAsInstructor([FromForm] RegisterInstructorDto register)
        {
            if (ModelState.IsValid)
            {
                // Save files
                var nationalCardFilePath = await SaveFileAsync(register.NationalCardImage);
                var cvPdfFilePath = await SaveFileAsync(register.CV_PDF);

                if (nationalCardFilePath == null || cvPdfFilePath == null)
                {
                    ModelState.AddModelError("", "File upload failed");
                    return BadRequest(ModelState);
                }

                // Map and create the ApplicationUser and Instructor
                var applicationUser = _mapper.Map<ApplicationUser>(register);
                Instructors instructor = new Instructors
                {
                    applicationUser = applicationUser,
                    CV_PDF_URL = cvPdfFilePath,
                    NationalCardImageURL = nationalCardFilePath,
                    SpecializationsID = register.SpecializationID,
                    BIO = register.Bio,
                    Status = InstructorStatus.Pending, // Default status as Pending
                    UserID = applicationUser.Id,
                    PhoneNumber = register.PhoneNumber
                };

                // Add the user
                var result = await userManager.CreateAsync(applicationUser, register.Password);

                if (result.Succeeded)
                {
                    // Ensure the "Instructor" role exists in the database
                    var roleExists = await roleManager.RoleExistsAsync("Instructor");
                    if (!roleExists)
                    {
                        var roleCreationResult = await roleManager.CreateAsync(new IdentityRole("Instructor"));
                        if (!roleCreationResult.Succeeded)
                        {
                            // Handle role creation errors
                            foreach (var error in roleCreationResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return BadRequest(ModelState);
                        }
                    }

                    // Assign the "Instructor" role to the user
                    var roleAssignmentResult = await userManager.AddToRoleAsync(applicationUser, "Instructor");
                    if (!roleAssignmentResult.Succeeded)
                    {
                        // Handle role assignment errors
                        foreach (var error in roleAssignmentResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest(ModelState);
                    }

                    // Save the instructor in the database
                    await _repository.AddAsync(instructor);

                    return Ok(new
                    {
                        Message = "Instructor registered successfully",
                        InstructorId = instructor.ID,
                        CVPath = instructor.CV_PDF_URL,
                        NationalCardPath = instructor.NationalCardImageURL,
                        Status = instructor.Status.ToString()
                    });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(ModelState);
        }
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, $"{Path.GetRandomFileName()}_{file.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path to save in the database
            return $"/uploads/{Path.GetFileName(filePath)}";
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
