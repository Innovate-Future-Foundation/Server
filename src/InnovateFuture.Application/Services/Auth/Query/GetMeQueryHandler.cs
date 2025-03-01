using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Services.Auth.Query;

public class GetMeQueryHandler: IRequestHandler<GetMeQuery, GetMeDto>
{
    private readonly IProfileRepository _profileRepository;
    public GetMeQueryHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<GetMeDto> Handle(GetMeQuery query, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetUserByProfileId(query.ProfileId, cancellationToken);

        return new GetMeDto
        {
            DefaultProfile = profile
        };
    }
}