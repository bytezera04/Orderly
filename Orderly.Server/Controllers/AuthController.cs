
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Shared.Helpers;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _Context;

        private readonly UserManager<AppUser> _UserManager;

        private readonly SignInManager<AppUser> _SignInManager;

        public AuthController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _Context = context;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new UserInfoDto
                {
                    IsAuthenticated = true,
                    UserName = User.Identity.Name
                });
            }

            return Ok(new UserInfoDto { IsAuthenticated = false });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            System.Diagnostics.Debug.WriteLine($"Registered!");

            // Check model state

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"Model State Invalid");

                return BadRequest(ModelState);
            }

            // Create this new user

            AppUser user = new AppUser
            {
                PublicId = await PublicIdGeneration.GenerateUserId(_Context),

                FullName = model.FullName,

                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpperInvariant(),

                Email = model.Email,
                NormalizedEmail = model.Email.ToUpperInvariant()
            };

            // Attempt to register the user

            var result = await _UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Sign the user in

                await _SignInManager.SignInAsync(user, isPersistent: false);

                return Ok(new
                {
                    message = "User registered successfully"
                });
            }
            else
            {
                // Failed, attach errors (these are safe to display)

                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Check model state

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to sign the user in

            var result = await _SignInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "User signed in successfully"
                });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect username or password.");

                return BadRequest(ModelState);
            }
        }

        public class RegisterModel
        {
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
