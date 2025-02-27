using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.ToursController;

public class GetTourPaginatedResponse : IPaginatedResult<GetTourResponse>
{
    public GetTourResponse[] Data { get; set; }
    public Meta Meta { get; set; }
}