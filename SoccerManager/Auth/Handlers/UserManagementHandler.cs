using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SoccerManager.Auth.Requirements;
using SoccerManager.Data;
using SoccerManager.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoccerManager.Auth.Handlers
{
    public class UserManagementHandler : AuthorizationHandler<UserManagementRequirement>
    {
        private readonly ApplicationDbContext _context;

        public UserManagementHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserManagementRequirement requirement)
        {
            //Nur Admins dürfen die User sehen
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier)) return;
            var uname = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //var user = _context.Users.Where(u => u.UserName == uname).FirstOrDefaultAsync();
            //if (user == null) return;
            var person = await _context.Persons.Where(p => p.UserId == uname).FirstOrDefaultAsync();
            if (person == null) return;

            if (person.GetPersonTypes().Any(t => t == Models.PersonType.ADMINISTRATOR)) context.Succeed(requirement);

            //if (!context.User.HasClaim(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Rol)) return;

            //var role = context.User.FindFirst(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Rol).Value;
            //if(role == Helpers.Constants.Strings.JwtClaims.ApiAccess)
            //{
            //    context.Succeed(requirement);
            //}
        }
    }
}
