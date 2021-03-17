using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        // Used to query and save instances of Activity.
        // Basically a way to interact with database through code
        public DbSet<Activity> Activities { get; set; }
    }
}