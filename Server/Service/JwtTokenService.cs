using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.IdentityModel.Tokens;

namespace Server.Service;

// Lille service der laver et signeret JWT-token til en bruger ved login.
// Minimal opsætning: HS256 med symmetrisk nøgle fra konfiguration, 8 timers levetid.
public class JwtTokenService
{
    private readonly byte[] _key;

    public JwtTokenService(IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "Jwt:Key mangler. Sæt den via user-secrets eller miljøvariablen Jwt__Key.");
        _key = Encoding.UTF8.GetBytes(key);
    }

    public string CreateToken(Users user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
