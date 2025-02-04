using FluentValidation;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;
public class GetProfilesQueryValidator : AbstractValidator<GetProfilesQuery>
{
    public GetProfilesQueryValidator()
    { 
        When(x => x.Filters != null, () =>
        {
            RuleFor(x => x.Filters!.NameOrEmailOrPhone)
                .MaximumLength(100).WithMessage("Searching content must not exceed 100 characters.")
                .When(x => x.Filters!=null&&!string.IsNullOrEmpty(x.Filters.NameOrEmailOrPhone));
        });

        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Limit must be greater than zero.");
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithMessage("Offset must be zero or greater.")
            .When(x=>x.Offset.HasValue);
        
    }
}