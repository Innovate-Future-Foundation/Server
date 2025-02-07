namespace InnovateFuture.Application.Common.Models;

public interface IPaginatedRequest<TFilters>
{
     TFilters? Filters { get; set; }
     string? SearchKey  { get; set; }
     Sorting[]? Sortings { get; set; }
     int? Offset { get; set; }
     int Limit { get; set; }
}