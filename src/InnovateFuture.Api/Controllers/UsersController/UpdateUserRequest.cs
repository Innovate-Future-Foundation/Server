namespace InnovateFuture.Api.Controllers.UsersController;

public class UpdateUserRequest
{
    public Guid UserId { get; set; }
    public Guid? DefaultProfile{ get; set;}
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}