using SharedCore.Model.Tokens;

namespace TestAuth.Domain.Model.CQRS.Dtos.Tokens
{
    public class RenewTokenDto
    {
        public AccessTokenDto? AccessToken { get; set; }
        public RefreshTokenDto? RefreshToken { get; set; }
    }
}
