
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.Auth;

public class RegisterOrganisationAdminRequest
{
    public string OrgName { get;  set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public Address? Address { get;  set; }
    public string? OrgEmail { get;  set; }
    
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; }
    public string Password { get; set; } = null!;
}