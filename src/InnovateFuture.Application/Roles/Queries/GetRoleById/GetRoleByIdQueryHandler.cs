using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Roles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Roles.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<Role>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> Handle(
            GetRolesQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _roleRepository.GetAllAsync();
        }
    }
}
