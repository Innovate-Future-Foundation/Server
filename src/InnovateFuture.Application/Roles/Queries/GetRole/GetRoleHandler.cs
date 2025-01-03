using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;

namespace InnovateFuture.Application.Roles.Queries.GetRole;
public class GetRoleHandler : IRequestHandler<GetRoleQuery, Role>
{
    private readonly IRoleRepository _roleRepository;
    public GetRoleHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Role> Handle(GetRoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(query.RoleId);
        
        return role;
    }
}

