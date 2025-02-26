using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;

namespace InnovateFuture.Application.Tours.Commands.CreateTour;

public class CreateTourHandler : IRequestHandler<CreateTourCommand, Guid>
{
    private readonly ITourRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTourHandler(ITourRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTourCommand command, CancellationToken cancellationToken)
    {
        var tour = new Tour(
            command.OrgId,
            command.Title,
            command.Comment,
            command.Summary,
            command.Text,
            command.CoverImgUrl,
            command.StartDate,
            command.EndDate,
            command.Leader
        );

        await _tourRepository.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tour.Id;
    }
}