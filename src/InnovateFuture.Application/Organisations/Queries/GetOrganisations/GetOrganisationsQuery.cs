using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class GetOrganisationsQuery : IRequest<List<Organisation>>
{
    public string? OrderBy { get; set; } = "OrgName";  // default by OrgName
    public bool IsAscending { get; set; } = true;      // default ascending
}