using Microsoft.AspNetCore.Authorization;

namespace Restaurant2API.Authorization
{
    public class MinimumAgeRequirement :IAuthorizationRequirement
    {

        public int MinimumAge { get;}

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
