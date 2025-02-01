namespace InnovateFuture.Application.Common.Models;

public interface IPaginatedResult<TData>
{ 
     TData[] Data { get; set; } 
     Meta Meta { get; set; }
}