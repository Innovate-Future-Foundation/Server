using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;

public class UpdateOrganisationCommand : IRequest<Organisation>
{
    public Guid OrgId { get; set; }
    public string? OrgName { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Subscription { get; set; }
    public StatusEnum Status { get; set; }
}