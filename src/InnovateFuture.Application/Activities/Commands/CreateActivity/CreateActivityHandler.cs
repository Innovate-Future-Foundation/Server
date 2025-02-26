using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;

namespace InnovateFuture.Application.Activities.Commands.CreateActivity;

public class CreateActivityHandler : IRequestHandler<CreateActivityCommand, Guid>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateActivityHandler(IActivityRepository activityRepository, IUnitOfWork unitOfWork)
    {
        _activityRepository = activityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateActivityCommand command, CancellationToken cancellationToken)
    {
        var activity = new Activity(
            command.OrgId,
            command.Title,
            command.Comment,
            command.Summary,
            command.Text,
            command.Location,
            command.CoverImgUrl,
            command.StartTime,
            command.EndTime
        );

        await _activityRepository.AddAsync(activity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return activity.Id;
    }
}