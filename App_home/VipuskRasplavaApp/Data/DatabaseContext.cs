using Microsoft.EntityFrameworkCore;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RaspredeleniyeDutyaApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Variant> Variants { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Variant>(
                builder =>
                {
                    builder.ComplexProperty(v => v.Data);
                }
            );
        }
    }
}
