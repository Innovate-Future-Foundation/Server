using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;

namespace InnovateFuture.Application.Activities.Queries.GetActivity;

public class GetActivityHandler : IRequestHandler<GetActivityQuery, Activity>
{
    private readonly IActivityRepository _activityRepository;

    public GetActivityHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Activity> Handle(GetActivityQuery query, CancellationToken cancellationToken)
    {
        return await _activityRepository.GetByIdAsync(query.ActivityId, cancellationToken);
    }
}