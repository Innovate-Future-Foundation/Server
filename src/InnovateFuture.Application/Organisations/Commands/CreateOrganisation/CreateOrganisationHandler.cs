using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationHandler : IRequestHandler<CreateOrganisationCommand, Guid>
{
    private readonly IOrgRepository _orgRepository;

    public CreateOrganisationHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<Guid> Handle(CreateOrganisationCommand command, CancellationToken cancellationToken)
    {
        // Create organisation
        var organisation = new Organisation(
            command.OrgName,
            command.LogoUrl,
            command.WebsiteUrl,
            command.Address,
            command.Email,
            command.Subscription
        );
        
        // Save to database
        await _orgRepository.AddAsync(organisation);

        return organisation.OrgId;
    }
}