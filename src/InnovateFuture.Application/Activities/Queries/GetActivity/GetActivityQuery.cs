using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Activities.Queries.GetActivity;

public class GetActivityQuery : IRequest<Activity>
{
    public Guid ActivityId { get; set; }
}