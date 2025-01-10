namespace InnovateFuture.Api.Controllers.UsersController;

public class QueryUsersRequest
{
    public Guid? CognitoUuid { get; set; }
    public Guid? OrgId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? InvitedByProfile { get; set; }
    public Guid? SupervisedByProfile { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}