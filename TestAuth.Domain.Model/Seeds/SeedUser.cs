﻿namespace TestAuth.Domain.Model.Seeds
{
    public class SeedUser
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public List<string>? Roles { get; set; }
    }
}
