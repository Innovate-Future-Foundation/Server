using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Auth.Queries.GetMe;

public class GetMeQuery: IRequest<Profile>
{
    public Guid ProfileId { get; }

    public GetMeQuery(Guid profileId)
    {
        ProfileId = profileId;
    }
}