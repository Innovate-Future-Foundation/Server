using System.Linq.Expressions;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;

namespace InnovateFuture.Application.Roles.Queries.GetRoles;
public class GetRolesHandler : IRequestHandler<GetRolesQuery, IEnumerable<Role>>
{
    private readonly IRoleRepository _roleRepository;
    public GetRolesHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<Role>> Handle(GetRolesQuery? query, CancellationToken cancellationToken)
    {
        Expression<Func<Role, bool>> queryPredicate;
        if (query == null)
        {
            queryPredicate = r => true; // Fetch all Roles
        }
        else
        {
            queryPredicate = r =>
                (string.IsNullOrEmpty(query.Name) || r.Name == query.Name) &&
                (!query.CodeName.HasValue || r.CodeName == query.CodeName);
        }

        var roles = await _roleRepository.GetAnyAsync(queryPredicate);
        
        return roles;
    }
}

