using MediatR;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public string? AvatarUrl { get; set; } 
    public bool? IsActive { get; set; }
    public bool? IsConfirmed { get; set; }
}

