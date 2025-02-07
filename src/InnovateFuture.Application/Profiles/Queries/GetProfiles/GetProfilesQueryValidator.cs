using FluentValidation;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;
public class GetProfilesQueryValidator : AbstractValidator<GetProfilesQuery>
{
    public GetProfilesQueryValidator()
    { 
        RuleFor(x => x.SearchKey)
            .MaximumLength(100).WithMessage("Search content must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.SearchKey));

        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Limit must be greater than zero.");
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithMessage("Offset must be zero or greater.")
            .When(x=>x.Offset.HasValue);
        
    }
}