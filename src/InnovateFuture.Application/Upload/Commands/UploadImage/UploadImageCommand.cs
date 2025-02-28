using MediatR;

namespace InnovateFuture.Application.Upload.Commands.UploadImage;
public class UploadImageCommand : IRequest<string>
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream FileStream { get; set; }
}