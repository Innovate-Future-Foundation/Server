using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Tours.Queries.GetTour;

public class GetTourQuery : IRequest<Tour>
{
    public Guid TourId { get; set; }
}