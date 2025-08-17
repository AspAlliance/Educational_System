using AutoMapper;
using Educational_System.Dto;
using Educational_System.Helpers;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;
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
        private readonly EmailService _emailService;
        public AuthController(EmailService emailService,
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Instructors> _repository,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager // Inject RoleManager
        )
        {
            _emailService = emailService;
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
                    var roleExists = await roleManager.RoleExistsAsync("user");
                    if (!roleExists)
                    {
                        var roleCreationResult = await roleManager.CreateAsync(new IdentityRole("user"));
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

                        await userManager.AddToRoleAsync(applicationUser, "user");

                    // Generate email confirmation token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    if (string.IsNullOrEmpty(token))
                        return BadRequest("Error generating email confirmation token.");

                    // Create the email confirmation link manually
                    var confirmationLink = $"http://localhost:27674/api/confirm-email?token={token}&email={applicationUser.Email}";

                    // Send confirmation link via email
                    await _emailService.SendEmailConfirmationAsync(register.Email, confirmationLink);
                    //set the user role is "user"
                   
                    return Ok(new
                    {
                        message = "User registered successfully. Please check your email to confirm your registration.",
                        token = token,
                        userinfo = new
                        {
                            Id = applicationUser.Id,
                            Username = applicationUser.UserName,
                            Name = applicationUser.Name,
                            Email = applicationUser.Email,
                            ProfileImg = applicationUser.ProfileImageURL,
                            Role = "user" // Default role
                        }
                    });
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token or email.");
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest("Error confirming email.");
        }
        [Authorize("admin")]
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
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                    return Ok(new
                    {
                        message = "Instructor registered successfully. Please check your email to confirm your registration.",
                        token = token,
                        userinfo = new
                        {
                            Id = applicationUser.Id,
                            Username = applicationUser.UserName,
                            Email = applicationUser.Email,
                            Name = applicationUser.Name,
                            ProfileImg = applicationUser.ProfileImageURL,
                            Role = "Instructor",
                            InstructorId = instructor.ID,
                            CVPath = instructor.CV_PDF_URL,
                            NationalCardPath = instructor.NationalCardImageURL,
                            Status = instructor.Status.ToString()
                        }
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
                            expiration = DateTime.Now.AddHours(1),
                            userinfo = new
                            {
                                Id = userfromdb.Id,
                                Username = userfromdb.UserName,
                                Name = userfromdb.Name,
                                Email = userfromdb.Email,
                                ProfileImg = userfromdb.ProfileImageURL,
                                Role = UserRole.FirstOrDefault() // Assuming the user has only one role
                            }
                        });
                    }
                }

                ModelState.AddModelError("username", "username or password is wrong");
            }

            return BadRequest(ModelState);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            try
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Ok(new { Message = "If the email exists, a reset link has been sent." });

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Something went wrong");

                var resetLink = $"http://localhost:27674/reset-password?token={token}&email={user.Email}";

                // Send the reset link via email
                await _emailService.SendResetPasswordEmail(request.Email, resetLink);

                return Ok(new { Message = "If the email exists, a reset link has been sent.", token = token });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return BadRequest("User not found");

                // Verify the token
                var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
                if (!result.Succeeded)
                    return BadRequest("Invalid or expired token");

                return Ok(new
                {
                    Message = "Password reset successfully."
                });
            }

            return BadRequest("Invalid payload");
        }


        [HttpPost(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user by their username
            var user = await userManager.FindByNameAsync(changePasswordDto.UserName);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Change the user's password
            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password changed successfully" });
            }

            // Handle errors if password change fails
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(SignOut))]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok("Signed out succussfully");
        }
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount(LoginBS request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Find the user by username
            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the password is correct
            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return Unauthorized("Invalid password.");
            }

            // Delete the user account
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("Account deleted successfully.");
            }

            // Return errors if deletion fails
            return BadRequest("Error deleting the account.");
        }
        [HttpPatch("update-instructor-status/{id}")]
        public async Task<IActionResult> UpdateInstructorStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the status value is valid
            if (!Enum.IsDefined(typeof(InstructorStatus), statusDto.Status))
            {
                return BadRequest("Invalid status value. Valid values are 0 (Pending), 1 (Approved), 2 (Rejected).");
            }

            // Find the instructor by ID
            var instructor = await _repository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound(new { Message = "Instructor not found" });
            }

            // Update the status
            instructor.Status = (InstructorStatus)statusDto.Status;

            try
            {
                await _repository.UpdateAsync(instructor);
                return Ok(new { Message = "Instructor status updated successfully", NewStatus = instructor.Status.ToString() });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while updating the status", Details = ex.Message });
            }
        }
    }
}
