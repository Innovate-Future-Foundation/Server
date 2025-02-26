using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Application.Activities.Queries.GetActivities;

public class GetActivitiesHandler : IRequestHandler<GetActivitiesQuery, (List<Activity> data, int totalItems)>
{
    private readonly IActivityRepository _activityRepository;

    public GetActivitiesHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<(List<Activity> data, int totalItems)> Handle(GetActivitiesQuery query, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Activity>(true);
        string? queryOrderBy = null;

        var queriesEmpty = query.GetType().GetProperties().All(p => p.GetValue(query) == null);

        if (!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                predicate = predicate.And(a =>
                    (!filters.OrgId.HasValue || a.OrgId == filters.OrgId) &&
                    (!filters.StartDate.HasValue || a.StartTime >= filters.StartDate) &&
                    (!filters.EndDate.HasValue || a.EndTime <= filters.EndDate));
            }

            if (!string.IsNullOrEmpty(query.SearchKey))
            {
                var searchKey = query.SearchKey;
                predicate = predicate.And(a =>
                    EF.Functions.ILike(a.Title, $"%{searchKey}%") ||
                    (!string.IsNullOrEmpty(a.Location) && EF.Functions.ILike(a.Location, $"%{searchKey}%")));
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

        return await _activityRepository.GetAnyAsync(predicate, query.Limit, query.Offset ?? 0, queryOrderBy);
    }
}