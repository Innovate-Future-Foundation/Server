using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.OrganisationsController;

public class CreateOrganisationRequest
{
    public string OrgName { get;  set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public Address? Address { get;  set; }
    public string? OrgEmail { get;  set; }
}