using System;
using System.Threading;
using System.Threading.Tasks;
using InnovateFuture.Roles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(
            UpdateRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.RoleId} was not found.");
            }

            role.Update(request.Name, request.Description);
            await _roleRepository.UpdateAsync(role);

            return Unit.Value;
        }
    }
}
