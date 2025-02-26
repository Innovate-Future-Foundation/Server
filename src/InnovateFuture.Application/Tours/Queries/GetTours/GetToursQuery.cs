using InnovateFuture.Application.Common.Models;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Tours.Queries.GetTours;

public class GetToursQuery : IRequest<(List<Tour> data, int totalItems)>
{
    public QueryTourFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryTourFilters
{
    public Guid? OrgId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TourStatusEnum? Status { get; set; }
    public Guid? Leader { get; set; }
}