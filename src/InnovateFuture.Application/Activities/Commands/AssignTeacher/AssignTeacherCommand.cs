using MediatR;

namespace InnovateFuture.Application.Activities.Commands.AssignTeacher;

public class AssignTeacherCommand : IRequest
{
    public Guid ActivityId { get; set; }
    public Guid TeacherId { get; set; }
}