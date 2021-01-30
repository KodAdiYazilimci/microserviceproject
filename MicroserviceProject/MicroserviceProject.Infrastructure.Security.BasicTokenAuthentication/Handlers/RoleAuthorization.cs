using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Requirements;
using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Util;
using MicroserviceProject.Model.Security;

using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Handlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RolesRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            if (context.HasSucceeded)
            {
                return Task.CompletedTask;
            }

            //if (context.User.IsInRole("Super Admin"))
            //{
            //    StateHandler.Succeed(context, requirement.Identifier);
            //    return Task.CompletedTask;
            //}

            if (context.User == null || requirement == null || string.IsNullOrWhiteSpace(requirement.Roles))
            {
                return Task.CompletedTask;
            }

            var requirementTokens = requirement.Roles.Split("|", StringSplitOptions.RemoveEmptyEntries);

            if (requirementTokens?.Any() != true)
            {
                return Task.CompletedTask;
            }

            var expectedRequirements = requirementTokens.ToList();

            if (expectedRequirements.Count == 0)
            {
                return Task.CompletedTask;
            }

            if (requirementTokens.Any(x => x == "Admin")
                &&
                JsonConvert.DeserializeObject<User>(context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData).Value).IsAdmin)
            {
                StateHandler.Succeed(context, requirement.Identifier);
                return Task.CompletedTask;
            }


            var userRoleClaims = context.User.Claims?.Where(c =>
                string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase)
                || string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));

            foreach (var claim in userRoleClaims ?? Enumerable.Empty<Claim>())
            {
                var match = expectedRequirements
                    .Where(r => string.Equals(r, claim.Value, StringComparison.OrdinalIgnoreCase));

                if (match.Any())
                {
                    StateHandler.Succeed(context, requirement.Identifier);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
