namespace InnovateFuture.Application.Services.Auth.TokenService;

public interface ITokenService
{
    Task<string> GenerateJwtTokenAsync(Guid profileId);
}