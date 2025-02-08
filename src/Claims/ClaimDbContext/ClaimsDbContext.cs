using Claims.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.ClaimDbContext
{
    public class ClaimsDbContext : DbContext
    {
        public ClaimsDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
