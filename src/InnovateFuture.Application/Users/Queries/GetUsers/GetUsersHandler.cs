using System.Linq.Expressions;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Queries.GetUsers;
public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;
    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Handle(GetUsersQuery? query, CancellationToken cancellationToken)
    {
        Expression<Func<User, bool>> queryPredicate;
        if (query == null)
        {
            queryPredicate = u => true; // Fetch all users
        }
        else
        {
            // Build predicate based on query conditions
            queryPredicate = u =>
                (query.CognitoUuid == Guid.Empty || u.CognitoUuid == query.CognitoUuid) &&
                (query.OrgId == Guid.Empty || u.Profiles.Any(p => p.OrgId == query.OrgId)) &&
                (query.RoleId == Guid.Empty || u.Profiles.Any(p => p.RoleId == query.RoleId)) &&
                (!query.InvitedByProfile.HasValue || u.Profiles.Any(p => p.InvitedBy == query.InvitedByProfile)) &&
                (!query.SupervisedByProfile.HasValue || u.Profiles.Any(p => p.SupervisedBy == query.SupervisedByProfile)) &&
                (string.IsNullOrEmpty(query.Email) || u.Email == query.Email) &&
                (string.IsNullOrEmpty(query.GivenName) || u.GivenName == query.GivenName) &&
                (string.IsNullOrEmpty(query.FamilyName) || u.FamilyName == query.FamilyName) &&
                (string.IsNullOrEmpty(query.Phone) || u.Phone == query.Phone) &&
                (!query.Birthday.HasValue || u.Birthday == query.Birthday);
        }

        var users = await _userRepository.GetAnyAsync(queryPredicate);
        
        return users;
    }
}

