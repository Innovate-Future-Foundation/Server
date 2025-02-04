namespace InnovateFuture.Api.Controllers.UsersController;

public class QueryUsersRequest
{
    public Guid? CognitoUuid { get; set; }
    public Guid? OrgId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? InviterProfile { get; set; }
    public Guid? SupervisorProfile { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 

}