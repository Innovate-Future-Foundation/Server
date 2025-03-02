using InnovateFuture.Api.Controllers.Organisations;

namespace InnovateFuture.Api.Controllers.Auth;

public class GetMeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string RoleCode { get; set; }
    public bool IsActive { get; set; }
    public bool IsConfirmed { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public GetOrganisationsResponse? Organisation { get; set; }
    public DateTime CreatedAt{ get;  set; }
    public DateTime UpdatedAt { get;  set; }
}