using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Services.Auth.Register;

public class RegisterNormalUserCommand: IRequest<(string UserName, string UserEmail, Guid ProfileId, string Token, RoleEnum RoleEnum)?>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public RoleEnum RoleEnum { get; set; }
    public Guid InviterProfileId { get; set; }
}