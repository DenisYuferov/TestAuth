using System.Security.Claims;
using TestAuth.Domain.Model.Dtos.Tokens;

namespace TestAuth.Domain.Abstraction.Providers
{
    public interface IJwtProvider
    {
        AccessTokenDto CreateAccessToken(List<Claim>? claims = null);
        RefreshTokenDto CreateRefreshToken();
    }
}
