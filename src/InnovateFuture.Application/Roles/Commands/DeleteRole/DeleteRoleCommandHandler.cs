using System;
using System.Threading;
using System.Threading.Tasks;
using InnovateFuture.Roles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(
            DeleteRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.RoleId} was not found.");
            }

            await _roleRepository.DeleteAsync(request.RoleId);
            return Unit.Value;
        }
    }
}
