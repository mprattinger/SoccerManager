using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SoccerManager.Data;
using SoccerManager.Data.DTO.Identity;

namespace SoccerManager.Controllers
{
    [Authorize(Policy = "RequireElevatedRights")]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            IConfiguration config,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> RegisterUser(string email)
        {
            try
            {
                _logger.LogInformation($"Checking email ({email})....");
                if (String.IsNullOrEmpty(email)) return BadRequest("Email darf nicht leer sein!");

                _logger.LogInformation($"Creating user...");
                var user = new User { Email = email, IsProductivePassword = false };
                var res = await _userManager.CreateAsync(user, "InitialPw");
                if (res.Succeeded)
                {
                    _logger.LogInformation($"Adding user to default role...");
                    var res2 = await _userManager.AddToRoleAsync(user, "user");
                    if (res2.Succeeded)
                    {
                        _logger.LogInformation("Everything ok!");
                        return Ok(new { registeredUser = user, role = "user" });
                    }
                    else
                    {
                        _logger.LogError("Error adding user to role!");
                        return StatusCode(500, "Internal server error");
                    }
                }
                else
                {
                    _logger.LogError("Error creating User!");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating User!", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error listening Users!", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("deleteuser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                _logger.LogDebug($"Loading user by userid {userId}...");
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogError($"Can't find user by id {userId}");
                    return NotFound();
                }
                _logger.LogDebug("Loading role of the user...");
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation("Removing roles...");
                var res = await _userManager.RemoveFromRolesAsync(user, roles);
                if (res.Succeeded)
                {
                    _logger.LogInformation("Removing user...");
                    var res2 = await _userManager.DeleteAsync(user);
                    if (res2.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        _logger.LogError("Error deleting user!");
                        return StatusCode(500, "Internal server error");
                    }
                }
                else
                {
                    _logger.LogError("Error deleting user roles!");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting User!", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}