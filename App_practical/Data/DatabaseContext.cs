using Microsoft.EntityFrameworkCore;

namespace App_practical.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Variant> Variants { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
