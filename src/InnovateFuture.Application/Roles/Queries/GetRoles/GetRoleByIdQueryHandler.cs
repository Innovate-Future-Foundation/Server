using System;
using System.Threading;
using System.Threading.Tasks;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Roles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Role>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(
            GetRoleByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.RoleId} was not found.");
            }
            return role;
        }
    }
}
