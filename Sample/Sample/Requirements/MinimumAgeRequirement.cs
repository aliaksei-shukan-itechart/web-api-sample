using Microsoft.AspNetCore.Authorization;

namespace Sample.Web.Requirements
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int age)
        {
            MinimumAge = age;
        }
    }
}
