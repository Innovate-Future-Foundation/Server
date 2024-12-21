
namespace InnovateFuture.Api.Controllers.ProfilesController;

public class GetProfileResponse
{
    public Guid ProfileId { get; set; }
    public string? InvitedBy { get; set; }
    public string? SupervisedBy { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public bool IsActive { get; set; }
}