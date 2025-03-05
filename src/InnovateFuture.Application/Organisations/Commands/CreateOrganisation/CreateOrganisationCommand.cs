using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<Guid>
{
    // Organisation
    public string OrgName { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public Address? Address { get; set; }
    public string? Email { get; set; } 
    public string? Subscription { get; set; }
}