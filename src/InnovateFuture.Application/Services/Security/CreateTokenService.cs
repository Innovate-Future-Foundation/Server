using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InnovateFuture.Api.Configs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InnovateFuture.Application.Services.Security;

public class CreateTokenService
{
    private readonly JWTConfig _jWTConfig;

    public CreateTokenService(IOptions<JWTConfig> options)
    {
        _jWTConfig = options.Value;
    }

    public string CreateToken(Dictionary<string, string> playBody)
    {
        var claims = new List<Claim>();
        foreach (var item in playBody)
            claims.Add(new Claim(item.Key, item.Value));

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jWTConfig.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jWTConfig.Issuer,
            audience: _jWTConfig.Audience,
            claims: claims,
            expires: DateTimeOffset.Now.LocalDateTime.AddSeconds(_jWTConfig.ExpireSeconds),
            signingCredentials: creds);

        var jwtHandel = new JwtSecurityTokenHandler();
        var accessToken = jwtHandel.WriteToken(token);

        return accessToken;
    }
}