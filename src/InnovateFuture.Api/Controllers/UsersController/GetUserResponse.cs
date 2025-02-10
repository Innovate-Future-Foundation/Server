
namespace InnovateFuture.Api.Controllers.UsersController;
public class GetUserResponse
{
    
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}