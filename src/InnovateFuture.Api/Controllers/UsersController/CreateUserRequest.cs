using System.ComponentModel.DataAnnotations;

namespace InnovateFuture.Api.Controllers.UsersController;

public class CreateUserRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public Guid OrgId { get; set; }
    public string RoleCode { get; set; }
    
    public Guid? Inviter { get; set; }
    public Guid? Supervisor { get; set; }
}