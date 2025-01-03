
using InnovateFuture.Api.Controllers.UsersController;

namespace InnovateFuture.Api.Controllers.OrganisationsController;

public class GetOrganisationResponse
{
    public Guid OrgId { get;  set; }
    public string OrgName { get; set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public string? Address { get;  set; }
    public string? Email { get;  set; }
    public string? Subscription { get;  set; }
    public string Status { get;  set; }
    public virtual ICollection<GetUserResponse>? Users { get; set; }
}