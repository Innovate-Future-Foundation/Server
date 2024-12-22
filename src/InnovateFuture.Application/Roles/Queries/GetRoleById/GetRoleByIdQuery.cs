using System.Collections.Generic;
using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Roles.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<List<Role>> { }
}
