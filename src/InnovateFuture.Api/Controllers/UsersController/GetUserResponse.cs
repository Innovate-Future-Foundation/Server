
namespace InnovateFuture.Api.Controllers.UsersController;
public class GetUserResponse
{
    public Guid UserId { get; set; }
    public Guid CognitoUuid { get; set; }
    public Guid DefaultProfile { get; set; }
    public string Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Phone { get; set; }
    public string? Birthday { get; set; }
}