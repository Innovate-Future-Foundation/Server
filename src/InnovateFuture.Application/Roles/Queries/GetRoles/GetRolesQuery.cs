using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Roles.Queries.GetRoles;

public class GetRolesQuery : IRequest<IEnumerable<Role>>
{
    public string? Name { get; set; }
    public RoleEnum? CodeName { get; set; }
}


