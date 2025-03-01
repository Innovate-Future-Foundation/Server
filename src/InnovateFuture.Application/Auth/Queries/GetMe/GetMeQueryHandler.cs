using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Auth.Queries.GetMe;

public class GetMeQueryHandler: IRequestHandler<GetMeQuery, Profile>
{
    private readonly IProfileRepository _profileRepository;
    public GetMeQueryHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<Profile> Handle(GetMeQuery query, CancellationToken cancellationToken)
    {
        return await _profileRepository.GetProfileByIdWithOrg(query.ProfileId, cancellationToken);
    }
}