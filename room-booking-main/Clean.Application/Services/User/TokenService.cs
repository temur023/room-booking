using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clean.Domain.Enitities;
using Clean.Permissions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Application.Services.Authentication;


public class TokenService(IConfiguration _config):ITokenService
{

    public Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("companyId", user.CompanyId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        if (RolePermissionMapping.RolePermissions.TryGetValue(user.Role, out var permissions))
        {
            foreach (var permission in permissions)
                claims.Add(new Claim("Permission", permission));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(jwtToken);
    }
}