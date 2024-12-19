using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation
{
    public class CreateOrganisationHandler : IRequestHandler<CreateOrganisationCommand, Guid>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public CreateOrganisationHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Guid> Handle(CreateOrganisationCommand command, CancellationToken cancellationToken)
        {
            var organisation = new Organisation(
                command.OrgName,
                command.Email,
                command.Address,
                command.LogoUrl,
                command.WebsiteUrl,
                command.Subscription
            );

            await _organisationRepository.AddAsync(organisation);
            return organisation.Id;
        }
    }
}
