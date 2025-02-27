using MediatR;

namespace InnovateFuture.Application.Activities.Commands.RemoveTeacher;

public class RemoveTeacherCommand : IRequest
{
    public Guid ActivityId { get; set; }
    public Guid TeacherId { get; set; }
}