namespace InnovateFuture.Application.Services.Security.TokenService;

public interface ITokenService
{
    Task<string> GenerateJwtTokenAsync(Guid profileId);
}