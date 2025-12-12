using Microsoft.EntityFrameworkCore;

namespace ToDo_list.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Tpdoitems> Tpdoitems { get; set; }
        
    }
}
