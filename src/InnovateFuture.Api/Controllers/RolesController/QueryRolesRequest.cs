using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Api.Controllers.RolesController;

public class QueryRolesRequest
{
    public string? Name { get; set; }
    public RoleEnum? CodeName { get; set; }
}