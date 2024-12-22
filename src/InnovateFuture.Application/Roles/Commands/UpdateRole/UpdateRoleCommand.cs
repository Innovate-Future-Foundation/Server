using System;
using MediatR;

namespace InnovateFuture.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
