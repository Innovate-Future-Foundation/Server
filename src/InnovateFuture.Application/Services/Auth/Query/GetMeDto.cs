using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Services.Auth.Query;

public class GetMeDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public Profile DefaultProfile { get; set; }
}