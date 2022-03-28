using Microsoft.AspNetCore.Authorization;

namespace Restaurant2API.Authorization
{
    public class CreatingMultipleRestourantRequirement : IAuthorizationRequirement
    {

        public int MinimumNum { get; }

        public CreatingMultipleRestourantRequirement(int minimumNum)
        {
            MinimumNum = minimumNum;
        }
    }
}
