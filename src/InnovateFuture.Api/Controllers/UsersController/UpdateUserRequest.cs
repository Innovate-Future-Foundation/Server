namespace InnovateFuture.Api.Controllers.UsersController;

public class UpdateUserRequest
{
    public Guid? CognitoUuid { get; set; }
    public Guid? DefaultProfile{ get; set;}
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}