using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InnovateFuture.Application.Services.Security.TokenService;

public class TokenService: ITokenService
{
    private readonly JWTConfig _jwtConfig;

    public TokenService(IOptions<JWTConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> GenerateJwtTokenAsync(Guid profileId)
    {
        // configurations
        var secretKey = _jwtConfig.SecretKey;
        var issuer = _jwtConfig.Issuer;
        var audience = _jwtConfig.Audience;
        var expireSeconds = int.Parse(_jwtConfig.ExpireSeconds);
       
        // profile Id cannot be null
        if (profileId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(profileId), "Profile Id cannot be empty.");
        }
        
        // claims
        var claims = new List<Claim>
        {
            new Claim("ProfileId", profileId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(expireSeconds),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}