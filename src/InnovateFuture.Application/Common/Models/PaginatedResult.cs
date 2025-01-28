namespace InnovateFuture.Application.Common.Models;

public class PaginatedResult<T>
{
    public T[] Data { get; set; }
    public Meta Meta { get; set; }
}