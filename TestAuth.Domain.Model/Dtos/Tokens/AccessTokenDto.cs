namespace TestAuth.Domain.Model.Dtos.Tokens
{
    public class AccessTokenDto
    {
        public string? Token { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
