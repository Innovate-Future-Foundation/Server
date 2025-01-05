using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

namespace InnovateFuture.Application.Profiles.Queries.GetProfile;
public class GetProfileHandler : IRequestHandler<GetProfileQuery, Profile>
{
    private readonly IProfileRepository _ProfileRepository;
    public GetProfileHandler(IProfileRepository ProfileRepository)
    {
        _ProfileRepository = ProfileRepository;
    }

    public async Task<Profile> Handle(GetProfileQuery query, CancellationToken cancellationToken)
    {
        var profile = await _ProfileRepository.GetByIdAsync(query.ProfileId);
        return profile;
    }
}

