using InnovateFuture.Domain.Enums;
    
namespace InnovateFuture.Api.Controllers.OrganisationsController;

public class QueryOrganisationsRequest
{
    public string? OrgName { get; set; }
    public StatusEnum? Status { get; set; } 
    public string? Email { get; set; }
}