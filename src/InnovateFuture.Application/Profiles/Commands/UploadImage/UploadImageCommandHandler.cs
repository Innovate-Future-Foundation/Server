using InnovateFuture.Application.Profiles.Commands.UploadImage;
using InnovateFuture.Application.Services.S3;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
{
    private readonly S3Service _s3Service;

    public UploadImageCommandHandler(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _s3Service.UploadAvatarAsync(request.FileStream, request.FileName);
        return imageUrl;
    }
}