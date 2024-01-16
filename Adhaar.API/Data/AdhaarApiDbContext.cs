using Adhaar.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Adhaar.API.Data
{
    public class AdhaarApiDbContext:DbContext
    {
        public AdhaarApiDbContext(DbContextOptions<AdhaarApiDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ImageAd> Images { get; set; }    
    }
}
