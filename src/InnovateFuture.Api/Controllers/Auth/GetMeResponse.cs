using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Controllers.Auth;

public class GetMeResponse
{
    public Profile DefaultProfile { get; set; }
}