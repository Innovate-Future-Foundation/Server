using System;
using System.Threading;
using System.Threading.Tasks;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Roles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Guid> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var role = new Role(request.Name, request.CodeName, request.Description);
            await _roleRepository.AddAsync(role);
            return role.RoleId;
        }
    }
}
