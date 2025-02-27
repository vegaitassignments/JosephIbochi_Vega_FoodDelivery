using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Authentication.Helper;

public static class GenerateToken
{
    public static async Task<string> CreateToken(IConfiguration config, ApplicationUser user, UserManager<ApplicationUser> userManager, double tokenDuration)
    {
        var jwtSectionSetting = config.GetSection("JwtSettings");
        var securityKey = Encoding.ASCII.GetBytes(jwtSectionSetting["Key"] ?? string.Empty);

        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var userRoles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(tokenDuration),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public static string ValidateToken(IConfiguration config, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSectionSetting = config.GetSection("JwtSettings");
        var securityKey = Encoding.ASCII.GetBytes(jwtSectionSetting["Key"] ?? string.Empty);

        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(securityKey),
            ValidateLifetime = true,
        };

        try{
            var principal = tokenHandler.ValidateToken(token, validationParameter, out SecurityToken validatedToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
        catch(Exception ex) {
            return null;
        }
    }
}