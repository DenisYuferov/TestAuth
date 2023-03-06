namespace TestAuth.Domain.Model.Options
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public string? SecurityKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }

        public TimeSpan AccessTokenExpires { get; set; }
        public TimeSpan RefreshTokenExpires { get; set; }
    }
}