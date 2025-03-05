using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.Organisations;

public class CreateOrganisationRequest
{
    public string OrgName { get;  set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public Address? Address { get;  set; }
    public string? Email { get;  set; }
}