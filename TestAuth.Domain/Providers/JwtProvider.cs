using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuth.Domain.Model.Options;
using TestAuth.Domain.Abstraction.Providers;
using TestAuth.Domain.Model.Dtos.Tokens;

namespace TestAuth.Domain.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IOptions<JwtOptions> _options;

        public JwtProvider(IOptions<JwtOptions> options) 
        {
            _options = options;
        }

        public AccessTokenDto CreateAccessToken(List<Claim>? claims = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecurityKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _options.Value.Audience,
                Issuer = _options.Value.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow + _options.Value.AccessTokenExpires,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessTokenDto
            {
                Token = tokenHandler.WriteToken(token),
                Expires = new DateTimeOffset(tokenDescriptor.Expires.Value),
            };
        }

        public RefreshTokenDto CreateRefreshToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecurityKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow + _options.Value.RefreshTokenExpires,
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new RefreshTokenDto
            {
                Token = tokenHandler.WriteToken(token),
                Expires = new DateTimeOffset(tokenDescriptor.Expires.Value),
            };
        }
    }
}
