using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HotelGetApi
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options) 
        {
            
        }
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Country> Countries { get; set; }
    }
}
