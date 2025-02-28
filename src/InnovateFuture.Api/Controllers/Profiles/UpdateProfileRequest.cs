namespace InnovateFuture.Api.Controllers.Profiles;

public class UpdateProfileRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; } 
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; } 
    public bool? IsActive { get; set; }
    public bool? IsConfirmed { get; set; }
}