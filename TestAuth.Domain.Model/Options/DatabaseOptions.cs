using TestAuth.Domain.Model.Seeds;

namespace TestAuth.Domain.Model.Options
{
    public class DatabaseOptions
    {
        public const string Database = "Database";

        public string? Connection { get; set; }

        public List<SeedUser>? SeedUsers { get; set; }
    }
}