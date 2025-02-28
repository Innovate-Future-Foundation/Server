using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Auth.Register;

public class RegisterOrganisationAdminCommand: IRequest<(Guid ProfileId, User User)?>
{
    // Organisation
    public string OrgName { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public Address? Address { get; set; }
    public string? OrgEmail { get; set; } 
    
    // User + Profile
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string Password { get; set; }
}