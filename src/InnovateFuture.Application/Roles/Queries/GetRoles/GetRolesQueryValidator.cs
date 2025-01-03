using FluentValidation;

namespace InnovateFuture.Application.Roles.Queries.GetRoles;
public class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}