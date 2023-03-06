namespace TestAuth.Domain.Model.Dtos.Tokens
{
    public class ObtainTokenDto
    {
        public AccessTokenDto? AccessToken { get; set; }
        public RefreshTokenDto? RefreshToken { get; set; }
    }
}
