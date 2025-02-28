using InnovateFuture.Api.Controllers.Organisations;
using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.Profiles;

public class GetProfileWithDetailsPaginatedResponse : IPaginatedResult<GetProfileWithDetailsResponse>
{
    public GetProfileWithDetailsResponse[] Data { get; set; }
    public Meta Meta { get; set; }
}


public class GetProfileWithDetailsResponse
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
    public GetProfileResponse? InviterProfile { get; set; }   
    public GetProfileResponse? SupervisorProfile { get; set; }
    public DateTime CreatedAt{ get;  set; }
    public DateTime UpdatedAt { get;  set; }
}

