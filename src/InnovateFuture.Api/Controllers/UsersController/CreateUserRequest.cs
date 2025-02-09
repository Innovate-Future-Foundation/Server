namespace InnovateFuture.Api.Controllers.UsersController;

public class CreateUserRequest
{
    public Guid OrgId { get; set; }
    public string RoleCode { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public Guid? Inviter  { get; set; }
    public Guid? Supervisor { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}