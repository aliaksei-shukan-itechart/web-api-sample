using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Sample.Web.Requirements.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "age"))
            {
                return Task.CompletedTask;
            }

            var age = Convert.ToInt32(context.User.FindFirst(c => c.Type == "age").Value);

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
