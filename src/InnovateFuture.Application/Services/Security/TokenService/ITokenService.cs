namespace InnovateFuture.Application.Services.Security.TokenService;

public interface ITokenService
{
    Task<string> GenerateJwtTokenAsync(Guid profileId);
    public Guid? GetProfileIdFromToken(string token);
}