using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
public class CreateOrganisationCommand : IRequest<Guid>
{
    [Required]
    public string OrgName { get; set; }

    public string LogoUrl { get; set; }

    public string WebsiteUrl { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Subscription { get; set; }

    public string Status { get; set; }
}

