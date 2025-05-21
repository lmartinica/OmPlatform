using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OmPlatform.Core;
using OmPlatform.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OmPlatform.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(IOptions<JwtSettings> jwtOptions) 
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateJwtToken(GetUserDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
