using InnovateFuture.Application.Common.Models;
using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Activities.Queries.GetActivities;

public class GetActivitiesQuery : IRequest<(List<Activity> data, int totalItems)>
{
    public QueryActivityFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryActivityFilters
{
    public Guid? OrgId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}