using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Roles.Queries.GetRole;

public class GetRoleQuery : IRequest<Role>
{
    public Guid RoleId { get; set; }
}


