using System;
using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IRequest<Role>
    {
        public Guid RoleId { get; set; }
    }
}
