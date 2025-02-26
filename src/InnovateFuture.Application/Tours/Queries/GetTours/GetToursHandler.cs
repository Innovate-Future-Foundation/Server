using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Application.Tours.Queries.GetTours;

public class GetToursHandler : IRequestHandler<GetToursQuery, (List<Tour> data, int totalItems)>
{
    private readonly ITourRepository _tourRepository;

    public GetToursHandler(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<(List<Tour> data, int totalItems)> Handle(GetToursQuery query, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Tour>(true);
        string? queryOrderBy = null;

        var queriesEmpty = query.GetType().GetProperties().All(p => p.GetValue(query) == null);

        if (!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                predicate = predicate.And(t =>
                    (!filters.OrgId.HasValue || t.OrgId == filters.OrgId) &&
                    (!filters.StartDate.HasValue || t.StartDate >= filters.StartDate) &&
                    (!filters.EndDate.HasValue || t.EndDate <= filters.EndDate) &&
                    (!filters.Status.HasValue || t.Status == filters.Status) &&
                    (!filters.Leader.HasValue || t.Leader == filters.Leader));
            }

            if (!string.IsNullOrEmpty(query.SearchKey))
            {
                var searchKey = query.SearchKey;
                predicate = predicate.And(t =>
                    EF.Functions.ILike(t.Title, $"%{searchKey}%") ||
                    (!string.IsNullOrEmpty(t.Summary) && EF.Functions.ILike(t.Summary, $"%{searchKey}%")));
            }

            if (query.Sortings != null && query.Sortings.Length > 0)
            {
                foreach (var sorting in query.Sortings)
                {
                    string orderBy = sorting.OrderBy;
                    string direction = sorting.IsAscending ? "asc" : "desc";
                    queryOrderBy += string.IsNullOrEmpty(queryOrderBy) ? $"{orderBy} {direction}" : $", {orderBy} {direction}";
                }
            }
        }

        return await _tourRepository.GetAnyAsync(predicate, query.Limit, query.Offset ?? 0, queryOrderBy);
    }
}