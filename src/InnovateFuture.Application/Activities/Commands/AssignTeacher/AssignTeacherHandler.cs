using MediatR;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

namespace InnovateFuture.Application.Activities.Commands.AssignTeacher;

public class AssignTeacherHandler : IRequestHandler<AssignTeacherCommand>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IProfileRepository _profileRepository;

    public AssignTeacherHandler(IActivityRepository activityRepository, IProfileRepository profileRepository)
    {
        _activityRepository = activityRepository;
        _profileRepository = profileRepository;
    }

    public async Task Handle(AssignTeacherCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
        var teacher = await _profileRepository.GetByIdAsync(command.TeacherId, cancellationToken);
        
        activity.AssignTeacher(teacher);
        await _activityRepository.UpdateAsync();
    }
}