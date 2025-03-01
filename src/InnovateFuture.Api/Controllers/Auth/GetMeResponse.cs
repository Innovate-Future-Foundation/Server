using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.Auth;

public class GetMeResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public Profile DefaultProfile { get; set; }
}