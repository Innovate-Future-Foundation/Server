namespace InnovateFuture.Api.Controllers.Users;

public class UpdateUserRequest
{
    public Guid? CognitoUuid { get; set; }
    public Guid? DefaultProfile{ get; set;}
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Birthday { get; set; } 
}