namespace TestAuth.Domain.Model.Dtos.Tokens
{
    public class RefreshTokenDto
    {
        public string? Token { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
