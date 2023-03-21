using TestAuth.Domain.Model.PostgreDb.Seeds;

namespace TestAuth.Domain.Model.PostgreDb.Options
{
    public class DatabaseOptions
    {
        public const string Database = "Database";

        public string? Connection { get; set; }

        public List<SeedUser>? SeedUsers { get; set; }
    }
}