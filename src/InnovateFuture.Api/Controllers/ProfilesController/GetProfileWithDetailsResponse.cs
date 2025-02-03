using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.ProfilesController;

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
    public Organisation? Org { get; set; }
    public string RoleName { get; set; }
    public bool IsActive { get; set; }
    public bool IsConfirmed { get; set; }
    public GetProfileResponse? Inviter { get; set; }    
    public GetProfileResponse? Supervisor { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt{ get;  set; }
    public DateTime UpdatedAt { get;  set; }
}

