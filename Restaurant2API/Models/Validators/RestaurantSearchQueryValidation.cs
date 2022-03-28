using FluentValidation;
using Restaurant2API.Entities;
using System.Linq;

namespace Restaurant2API.Models.Validators
{
    public class RestaurantSearchQueryValidation : AbstractValidator<RestaurantSearchQuery>
    {
        private int[] allowedPageSize = new[] {5, 10, 15 };

        private string[] allowedSortByColumnNames =
            {nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description)};
        public RestaurantSearchQueryValidation()
        {
            RuleFor(r=> r.PageNumber).GreaterThanOrEqualTo(0);


            RuleFor(r => r.PageSize).Custom((value, context) =>
             {
             if (value != null && !allowedPageSize.Contains((int)value))
                 {
                     context.AddFailure("PageSize", $"PageSize must be one of: {string.Join(",", allowedPageSize)}");
                 }
             });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in[ {string.Join(", ",allowedSortByColumnNames)} ]");
        }
    }
}
