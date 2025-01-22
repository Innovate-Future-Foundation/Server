
namespace InnovateFuture.Api.Controllers.RolesController;

public class GetRoleResponse
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string? Description { get; set; }
}