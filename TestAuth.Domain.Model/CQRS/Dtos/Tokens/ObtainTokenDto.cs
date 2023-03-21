using SharedCore.Model.Tokens;

namespace TestAuth.Domain.Model.CQRS.Dtos.Tokens
{
    public class ObtainTokenDto
    {
        public AccessTokenDto? AccessToken { get; set; }
        public RefreshTokenDto? RefreshToken { get; set; }
    }
}
