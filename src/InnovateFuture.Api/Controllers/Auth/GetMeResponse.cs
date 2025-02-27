using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.Auth;

public class GetMeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Profile DefaultProfile { get; set; }
}