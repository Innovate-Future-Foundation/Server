using MediatR;

namespace InnovateFuture.Application.Profiles.Commands.UploadImage;
public class UploadImageCommand : IRequest<string>
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream FileStream { get; set; }
}