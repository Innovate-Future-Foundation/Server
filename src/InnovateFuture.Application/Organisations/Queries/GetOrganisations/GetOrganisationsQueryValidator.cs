using FluentValidation;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;
public class GetOrganisationsQueryValidator : AbstractValidator<GetOrganisationsQuery>
{
    public GetOrganisationsQueryValidator()
    { 
        When(x => x.Filters != null, () =>
        {
            RuleFor(x => x.Filters!.OrgNameOrEmail)
                .MaximumLength(100).WithMessage("Organisation name must not exceed 100 characters.")
                .When(x => x.Filters!=null&&!string.IsNullOrEmpty(x.Filters.OrgNameOrEmail));
        });

        RuleFor(x => x.Limit)
            .GreaterThan(0).WithMessage("Limit must be greater than zero.");
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithMessage("Offset must be zero or greater.")
            .When(x=>x.Offset.HasValue);
        
    }
}