using Microsoft.EntityFrameworkCore;

namespace Products_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Models.Entity.Product> Products { get; set; }
    }
}
