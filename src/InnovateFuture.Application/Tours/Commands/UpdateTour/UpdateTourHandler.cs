using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;

namespace InnovateFuture.Application.Tours.Commands.UpdateTour;

public class UpdateTourHandler : IRequestHandler<UpdateTourCommand, Guid>
{
    private readonly ITourRepository _tourRepository;

    public UpdateTourHandler(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<Guid> Handle(UpdateTourCommand command, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(command.Id, cancellationToken);

        tour.UpdateTour(
            command.Title,
            command.Comment,
            command.Summary,
            command.Text,
            command.CoverImgUrl,
            command.StartDate,
            command.EndDate,
            command.Leader,
            command.Status
        );

        await _tourRepository.UpdateAsync();
        return tour.Id;
    }
}