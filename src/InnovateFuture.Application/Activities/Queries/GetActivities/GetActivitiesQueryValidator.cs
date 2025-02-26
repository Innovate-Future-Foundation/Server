using FluentValidation;

namespace InnovateFuture.Application.Activities.Queries.GetActivities;

public class GetActivitiesQueryValidator : AbstractValidator<GetActivitiesQuery>
{
    public GetActivitiesQueryValidator()
    {
        RuleFor(x => x.SearchKey)
            .MaximumLength(100).WithMessage("Search content must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SearchKey));

        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Limit must be greater than zero.");

        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithMessage("Offset must be zero or greater.")
            .When(x => x.Offset.HasValue);

        When(x => x.Filters?.StartDate != null && x.Filters?.EndDate != null, () =>
        {
            RuleFor(x => x.Filters.StartDate)
                .LessThan(x => x.Filters.EndDate)
                .WithMessage("Start date must be before end date.");
        });
    }
}