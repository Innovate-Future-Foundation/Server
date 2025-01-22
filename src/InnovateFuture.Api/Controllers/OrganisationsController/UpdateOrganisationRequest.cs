namespace InnovateFuture.Api.Controllers.OrganisationsController;

public class UpdateOrganisationRequest
{
    public string? OrgName { get;  set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public string? Address { get;  set; }
    public string? Email { get;  set; }
    public string? Subscription { get;  set; }
}