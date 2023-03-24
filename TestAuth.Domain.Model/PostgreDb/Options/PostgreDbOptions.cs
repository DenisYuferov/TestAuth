using TestAuth.Domain.Model.PostgreDb.Seeds;

namespace TestAuth.Domain.Model.PostgreDb.Options
{
    public class PostgreDbOptions
    {
        public const string PostrgreDb = "PostrgreDb";

        public string? Connection { get; set; }

        public List<SeedUser>? SeedUsers { get; set; }
    }
}