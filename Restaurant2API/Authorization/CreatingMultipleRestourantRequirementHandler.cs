using Microsoft.AspNetCore.Authorization;
using Restaurant2API.Entities;
using System.Security.Claims;

namespace Restaurant2API.Authorization
{
    public class CreatingMultipleRestourantRequirementHandler : AuthorizationHandler<CreatingMultipleRestourantRequirement>
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public CreatingMultipleRestourantRequirementHandler(RestaurantDbContext dbContext, ILogger<MinimumAgeRequirementHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatingMultipleRestourantRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var numFound = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (numFound >= requirement.MinimumNum)
            {
                _logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask;

        }
    }
}
