using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Profiles.Queries.GetProfile;

public class GetProfileQuery : IRequest<Profile>
{
    public Guid ProfileId { get; set; }
}


