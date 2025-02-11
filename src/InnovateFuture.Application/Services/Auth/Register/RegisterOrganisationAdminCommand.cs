using MediatR;

namespace InnovateFuture.Application.Services.Auth.Register;

public class RegisterOrganisationAdminCommand: IRequest<bool>
{
    // Organisation
    public string OrgName { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Address { get; set; }
    public string? OrgEmail { get; set; } 
    
    // User + Profile
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; }
    public string Password { get; set; } = null!;
}