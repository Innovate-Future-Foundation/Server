namespace InnovateFuture.Api.Controllers.UsersController;

public class CreateUserRequest
{
    public Guid OrgId { get; set; }
    public Guid RoleId { get; set; }
    public string Email { get; set; }
    public Guid? InvitedBy { get; set; }
    public Guid? SupervisedBy { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}