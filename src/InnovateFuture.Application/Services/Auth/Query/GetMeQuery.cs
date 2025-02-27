using MediatR;

namespace InnovateFuture.Application.Services.Auth.Query;

public class GetMeQuery: IRequest<GetMeDto>
{
    public Guid ProfileId { get; }

    public GetMeQuery(Guid profileId)
    {
        ProfileId = profileId;
    }
}