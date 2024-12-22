using System;
using MediatR;

namespace InnovateFuture.Application.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest
    {
        public Guid RoleId { get; set; }
    }
}
