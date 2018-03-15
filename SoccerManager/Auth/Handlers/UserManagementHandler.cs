using Microsoft.AspNetCore.Authorization;
using SoccerManager.Auth.Requirements;
using SoccerManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserManagementRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Rol)) return Task.CompletedTask;

            var role = context.User.FindFirst(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Rol).Value;
            if(role == Helpers.Constants.Strings.JwtClaims.ApiAccess)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
