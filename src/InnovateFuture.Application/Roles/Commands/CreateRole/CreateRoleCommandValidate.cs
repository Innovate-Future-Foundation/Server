using FluentValidation;

namespace InnovateFuture.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CodeName).GreaterThan(0);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
