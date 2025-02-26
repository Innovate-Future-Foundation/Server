using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;

namespace InnovateFuture.Application.Tours.Queries.GetTour;

public class GetTourHandler : IRequestHandler<GetTourQuery, Tour>
{
    private readonly ITourRepository _tourRepository;

    public GetTourHandler(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<Tour> Handle(GetTourQuery query, CancellationToken cancellationToken)
    {
        return await _tourRepository.GetByIdAsync(query.TourId, cancellationToken);
    }
}