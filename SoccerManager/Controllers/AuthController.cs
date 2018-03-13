using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SoccerManager.Auth;
using SoccerManager.Data.DTO.Identity;
using SoccerManager.Helpers;
using SoccerManager.Models;
using SoccerManager.ViewModels;

namespace SoccerManager.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await getClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity.identity == null && identity.loginStatus == LoginStatus.INVALID)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            if(identity.loginStatus == LoginStatus.LOGIN_NOT_ALLOWED) return BadRequest(Errors.AddErrorToModelState("login_notallowed", "Login is not allowed!", ModelState));
            if (identity.loginStatus == LoginStatus.NO_PRODUCTIVE_PASSWORD) return BadRequest(Errors.AddErrorToModelState("login_noprodpw", "Password needs to be changed!", ModelState));

            var jwt = await Tokens.GenerateJwt(identity.identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<(ClaimsIdentity identity, LoginStatus loginStatus)> getClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((null, LoginStatus.INVALID));

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null) return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((null, LoginStatus.INVALID));

            //Check user status
            if(!userToVerify.LoginAllowed) return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((null, LoginStatus.LOGIN_NOT_ALLOWED));

            //Check user pasword
            if (!userToVerify.IsProductivePassword) return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((null, LoginStatus.NO_PRODUCTIVE_PASSWORD));

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                //return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((userName, LoginStatus.OK));
                return await Task.FromResult((_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id), LoginStatus.OK));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<(ClaimsIdentity, LoginStatus)>((null, LoginStatus.INVALID));
        }

        //private async Task<ClaimsIdentity> getClaimsIdentity(string userName, string password)
        //{
        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        //        return await Task.FromResult<ClaimsIdentity>(null);

        //    // get the user to verifty
        //    var userToVerify = await _userManager.FindByNameAsync(userName);

        //    if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

        //    // check the credentials
        //    if (await _userManager.CheckPasswordAsync(userToVerify, password))
        //    {
        //        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
        //    }

        //    // Credentials are invalid, or account doesn't exist
        //    return await Task.FromResult<ClaimsIdentity>(null);
        //}
    }

    public enum LoginStatus
    {
        OK,
        INVALID,
        NO_PRODUCTIVE_PASSWORD,
        LOGIN_NOT_ALLOWED
    }
}