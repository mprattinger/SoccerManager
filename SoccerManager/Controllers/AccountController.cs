using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Data;
using SoccerManager.Data.DTO.Identity;
using SoccerManager.Extensions;
using SoccerManager.ViewModels;

namespace SoccerManager.Controllers
{
    //[Authorize(Policy = "RequireElevatedRights")]
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<User> userManager,
            ILogger<AccountController> logger,
            IConfiguration config,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _config = config;
            _context = context;
        }

        [HttpGet]
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

        //[AllowAnonymous]
        ////[HttpPost("/registeruser/{email}")]
        //[HttpPost("{email}")]
        //public async Task<IActionResult> RegisterUser(string email)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Checking email ({email})....");
        //        if (String.IsNullOrEmpty(email)) return BadRequest("Email darf nicht leer sein!");
        //        if(!email.IsValidEmail()) return BadRequest("Email muß eine Email-Addresse sein!");

        //        _logger.LogInformation($"Creating user...");
        //        var user = new User { Email = email, IsProductivePassword = false };
        //        var res = await _userManager.CreateAsync(user, "InitialPw");
        //        if (res.Succeeded)
        //        {
        //            _logger.LogInformation($"Adding user to default role...");
        //            var res2 = await _userManager.AddToRoleAsync(user, "user");
        //            if (res2.Succeeded)
        //            {
        //                _logger.LogInformation("Everything ok!");
        //                return Ok(new { registeredUser = user, role = "user" });
        //            }
        //            else
        //            {
        //                _logger.LogError("Error adding user to role!");
        //                return StatusCode(500, "Internal server error");
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError("Error creating User!");
        //            return StatusCode(500, "Internal server error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error creating User!", ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPost("{id}")]
        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    try
        //    {
        //        _logger.LogDebug($"Loading user by userid {id}...");
        //        var user = await _userManager.FindByIdAsync(id);
        //        if (user == null)
        //        {
        //            _logger.LogError($"Can't find user by id {id}");
        //            return NotFound();
        //        }
        //        _logger.LogDebug("Loading role of the user...");
        //        var roles = await _userManager.GetRolesAsync(user);
        //        _logger.LogInformation("Removing roles...");
        //        var res = await _userManager.RemoveFromRolesAsync(user, roles);
        //        if (res.Succeeded)
        //        {
        //            _logger.LogInformation("Removing user...");
        //            var res2 = await _userManager.DeleteAsync(user);
        //            if (res2.Succeeded)
        //            {
        //                return Ok();
        //            }
        //            else
        //            {
        //                _logger.LogError("Error deleting user!");
        //                return StatusCode(500, "Internal server error");
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError("Error deleting user roles!");
        //            return StatusCode(500, "Internal server error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error deleting User!", ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPost()]
        //public async Task<IActionResult> GetToken(LoginViewModel viewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.FindByEmailAsync(viewModel.Email);
        //            if (user != null)
        //            {
        //                var result = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Password, false);
        //                if (result.Succeeded)
        //                {
        //                    var claims = new[]
        //                    {
        //                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //                    };
        //                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        //                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
        //                        _config["Tokens:Issuer"],
        //                        claims,
        //                        expires: DateTime.Now.AddDays(30),
        //                        signingCredentials: creds);

        //                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        //                }
        //                else
        //                {
        //                    _logger.LogInformation($"Cannot login User");
        //                    return Unauthorized();
        //                }
        //            }
        //            else
        //            {
        //                _logger.LogInformation($"Cannot find user with email {viewModel.Email}");
        //                return NotFound();
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError($"Modelstate is not valid! ({ModelState.StringifyModelStateErrors()})");
        //            return BadRequest(ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error get token!", ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}