namespace InnovateFuture.Application.Services.Security.TokenService;

public class JWTConfig
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string ExpireSeconds { get; set; }

    public string Domain { get; set; }
}