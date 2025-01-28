using FluentValidation;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;
public class GetOrganisationsQueryValidator : AbstractValidator<GetOrganisationsQuery>
{
    public GetOrganisationsQueryValidator()
    { 
        When(x => x.Filters != null, () =>
        {
            RuleFor(x => x.Filters.OrgName)
                .MaximumLength(100).WithMessage("Organisation name must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Filters.OrgName));

            RuleFor(x => x.Filters.Email)
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .EmailAddress().WithMessage("Kindly enter a valid email address.")
                .When(x => !string.IsNullOrEmpty(x.Filters.Email));
        });

        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Limit must be greater than zero.");
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithMessage("Offset must be zero or greater.")
            .When(x=>x.Offset.HasValue);
        
    }
}