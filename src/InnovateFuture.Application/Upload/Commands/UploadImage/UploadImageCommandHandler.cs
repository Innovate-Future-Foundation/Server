using InnovateFuture.Application.Services.S3;
using MediatR;

namespace InnovateFuture.Application.Upload.Commands.UploadImage;
public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
{
    private readonly S3Service _s3Service;

    public UploadImageCommandHandler(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _s3Service.UploadImageAsync(request.FileStream, request.FileName, request.ContentType);
        return imageUrl;
    }
}