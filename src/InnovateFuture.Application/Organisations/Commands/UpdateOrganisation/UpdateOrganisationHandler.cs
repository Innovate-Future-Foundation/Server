using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;

public class UpdateOrganisationHandler : IRequestHandler<UpdateOrganisationCommand, Organisation>
{
    private readonly IOrgRepository _orgRepository;

    public UpdateOrganisationHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<Organisation> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisation = await _orgRepository.GetByIdAsync(request.OrgId);
        
        // organisation.UpdateOrganisationDetails(
        //     request.OrgName,
        //     request.LogoUrl,
        //     request.WebsiteUrl,
        //     request.Address,
        //     request.Email,
        //     request.Subscription,
        //     request.Status
        // );

        await _orgRepository.UpdateAsync();

        return organisation;
    }
}