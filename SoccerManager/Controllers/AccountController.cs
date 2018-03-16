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
using SoccerManager.Data.DTO;
using SoccerManager.Data.DTO.Identity;
using SoccerManager.Extensions;
using SoccerManager.ViewModels;
using SoccerManager.ViewModels.Auth;

namespace SoccerManager.Controllers
{
    //[Authorize(Policy = "RequireElevatedRights")]
    [Authorize(Policy = "UserManagement")]
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
                //var users = from u in _context.Users
                //        join p in _context.Persons on u.UserName equals p.UserId
                //        into joined
                //        select new UserListViewModel { UserName = u.UserName, EMail = u.Email, IsLoginAllowed = u.LoginAllowed, IsProductivePassword = u.IsProductivePassword, AssignedPersons = joined.ToList() };
                var d = new List<UserListViewModel>();
                await _context.Users.ForEachAsync(async u =>
                {
                    var vm = new UserListViewModel { UserName = u.UserName, EMail = u.Email, IsLoginAllowed = u.LoginAllowed, IsProductivePassword = u.IsProductivePassword };
                    vm.AssignedPersons = new List<Person>();
                    await _context.Persons.Where(p => p.UserId == u.UserName).ForEachAsync(p => vm.AssignedPersons.Add(p));
                    d.Add(vm);
                });

                return Ok(d);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error listening Users!", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        //[HttpPost("/registeruser/{email}")]
        [HttpPost()]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel regUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"Checking email ({regUser.UserName})....");
                    if (String.IsNullOrEmpty(regUser.UserName)) return BadRequest("Email darf nicht leer sein!");
                    if (!regUser.UserName.IsValidEmail()) return BadRequest("Email muß eine Email-Addresse sein!");

                    _logger.LogInformation($"Creating user...");
                    var user = new User { UserName = regUser.UserName, Email = regUser.UserName, IsProductivePassword = false, LoginAllowed = false };
                    var res = await _userManager.CreateAsync(user, "InitialPw");
                    if (res.Succeeded)
                    {
                        _logger.LogInformation("Everything ok!");
                        return Ok(new { registeredUser = user });
                    }
                    else
                    {
                        _logger.LogError("Error creating User!");
                        return StatusCode(500, "Internal server error");
                    }
                }
                else
                {
                    _logger.LogInformation($"Modelstate is not valid! ({ModelState.StringifyModelStateErrors()})");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating User!", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteUser([FromBody] RegisterUserViewModel regUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug($"Loading user by userid {regUser.UserName}...");
                    var user = await _userManager.FindByNameAsync(regUser.UserName);
                    if (user == null)
                    {
                        _logger.LogError($"Can't find user by id {regUser.UserName}");
                        return NotFound();
                    }

                    _logger.LogDebug("Loading related persons...");
                    var persons = await _context.Persons.Where(p => p.UserId == regUser.UserName).ToListAsync();
                    foreach (var p in persons)
                    {
                        _logger.LogDebug($"Changing person {p.FullName()}...");
                        p.UserId = String.Empty;
                    }
                    if (persons.Count() > 0)
                    {
                        _logger.LogInformation($"Updating {persons.Count()} persons...");
                        _context.UpdateRange(persons);
                        await _context.SaveChangesAsync();
                    }

                    _logger.LogInformation("Removing user...");
                    var res = await _userManager.DeleteAsync(user);
                    if (res.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        _logger.LogError($"Error deleting user! ({res.Errors.ToList().StringifyIdentityErrors()})");
                        return StatusCode(500, "Internal server error");
                    }
                }
                else
                {
                    _logger.LogInformation($"Modelstate is not valid! ({ModelState.StringifyModelStateErrors()})");
                    return BadRequest(ModelState);
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