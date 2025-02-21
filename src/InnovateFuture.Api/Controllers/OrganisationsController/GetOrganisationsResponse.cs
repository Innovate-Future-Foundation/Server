using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Api.Controllers.OrganisationsController;

public class GetOrganisationPaginatedResponse:IPaginatedResult<GetOrganisationsResponse>
{
    public GetOrganisationsResponse[] Data { get; set; }
    public Meta Meta { get; set; }
}

public class GetOrganisationsResponse
{
    public Guid Id { get;  set; }
    public string OrgName { get; set; }
    public string? LogoUrl { get;  set; }
    public string? WebsiteUrl { get;  set; }
    public Address? Address { get;  set; }
    public string? Email { get;  set; }
    public string SubscriptionCode { get;  set; }
    public string OrgStatusCode { get;  set; }
    public DateTime CreatedAt{ get;  set; }
    public DateTime UpdatedAt { get;  set; }
}