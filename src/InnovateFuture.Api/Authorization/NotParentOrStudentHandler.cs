using System.Text.Json;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InnovateFuture.Api.Authorization;

public class NotParentOrStudentHandler : IAuthorizationHandler
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotParentOrStudentHandler> _logger;

    public NotParentOrStudentHandler(IMediator mediator, ILogger<NotParentOrStudentHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        _logger.LogInformation("[STARTING OrgAdmin Authorization Handler]");

        var requirement = context.Requirements.OfType<NotParentOrStudentRequirement>().FirstOrDefault();
        if (requirement == null)
        {
            return;
        }

        var profileIdClaim = context.User.FindFirst("ProfileId")?.Value;
        if (string.IsNullOrEmpty(profileIdClaim) || !Guid.TryParse(profileIdClaim, out var profileId))
        {
            _logger.LogWarning("ProfileId claim is missing or invalid.");
            return;
        }

        var query = new GetProfileQuery { ProfileId = profileId };
        var profile = await _mediator.Send(query);
        if (profile?.Role != RoleEnum.Student)
        {
            _logger.LogInformation("Authorization Succeeded!");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("Authorization failed!");
            throw new IFUnauthorizedActionException("User role cannot be student.");
        }
    }
}
