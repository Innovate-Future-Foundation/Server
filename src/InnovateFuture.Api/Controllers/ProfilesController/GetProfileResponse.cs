using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.ProfilesController;

public class GetProfilePaginatedResponse : IPaginatedResult<GetProfileResponse>
{
    public GetProfileResponse[] Data { get; set; }
    public Meta Meta { get; set; }
}

public class GetProfileResponse
{
    public Guid ProfileId { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public Guid? OrgId { get; set; }
    public string? RoleCode { get; set; }
    public bool IsActive { get; set; }
    public bool IsConfirmed { get; set; }
    public Guid? Inviter { get; set; }
    public Guid? Supervisor { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt{ get;  set; }
    public DateTime UpdatedAt { get;  set; }
}