using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileCommand : IRequest<Guid>
{
    public Guid ProfileId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public string? Avatar { get; set; } 
    public bool? IsActive { get; set; }
}

