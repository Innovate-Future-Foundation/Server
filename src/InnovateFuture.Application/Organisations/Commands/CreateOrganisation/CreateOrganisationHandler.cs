using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationHandler : IRequestHandler<CreateOrganisationCommand, Guid>
{
    private readonly IOrgRepository _orgRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateOrganisationHandler(IOrgRepository orgRepository, IUnitOfWork unitOfWork)
    {
        _orgRepository = orgRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateOrganisationCommand command, CancellationToken cancellationToken)
    {
        // Create organisation
        var organisation = new Organisation(
            command.OrgName,
            null,
            command.LogoUrl,
            command.WebsiteUrl,
            command.Address,
            command.Email
            );
        
        // Save to database
        await _orgRepository.AddAsync(organisation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return organisation.Id;
    }
}