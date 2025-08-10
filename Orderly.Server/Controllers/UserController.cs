
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;

namespace Orderly.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _Context;

        private readonly UserManager<AppUser> _UserManager;

        public UserController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _Context = context;
            _UserManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            // Get the user

            AppUser? user = await _UserManager.GetUserAsync(User);

            if (user is null || user.Id is null)
            {
                return Unauthorized();
            }

            // Respond with user

            return Ok(user.ToDto());
        }
    }
}
