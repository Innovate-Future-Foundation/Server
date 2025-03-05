using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Upload.Commands.UploadImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovateFuture.Api.Controllers.Upload;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class UploadController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public UploadController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpPost("Image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var command = new UploadImageCommand
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileStream = file.OpenReadStream()
        };
        var imageUrl = await _mediator.Send(command);

        return Ok(new { Url = imageUrl });
    }
}