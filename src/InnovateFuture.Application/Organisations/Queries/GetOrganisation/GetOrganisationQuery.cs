using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisation
{
    public class GetOrganisationQuery : IRequest<Organisation>
    {
        public Guid OrganisationId { get; set; }
    }
}
