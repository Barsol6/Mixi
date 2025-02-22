using Microsoft.EntityFrameworkCore;
using mixi.Modules.Users;

namespace mixi.Modules.Database;

public class MixiDbContext:DbContext
{
    public MixiDbContext(DbContextOptions<MixiDbContext> options)
        :base(options){ } 
    
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
        
        base.OnModelCreating(modelBuilder);
    }
}