using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;

namespace InnovateFuture.Application.Activities.Commands.UpdateActivity;

public class UpdateActivityHandler : IRequestHandler<UpdateActivityCommand, Guid>
{
    private readonly IActivityRepository _activityRepository;

    public UpdateActivityHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Guid> Handle(UpdateActivityCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.Id, cancellationToken);

        activity.UpdateActivity(
            command.Title,
            command.Comment,
            command.Summary,
            command.Text,
            command.Location,
            command.CoverImgUrl,
            command.StartTime,
            command.EndTime,
            command.Status
        );

        await _activityRepository.UpdateAsync();
        return activity.Id;
    }
}