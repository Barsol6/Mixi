using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Database;

public class MSSQLMixiDbContext:DbContext
{
    public MSSQLMixiDbContext(DbContextOptions<MSSQLMixiDbContext> options)
        :base(options){ } 
    
    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity => 
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Username)
                .IsRequired();
            entity.Property(x => x.Password)
                .IsRequired();
            entity.Property(x => x.UserType)
                .IsRequired();
            entity.HasIndex(x => x.Username)
                .HasDatabaseName("IX_Users_Username");
            entity.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("IX_Users_CreatedAt");
        });
        

        
        base.OnModelCreating(modelBuilder);
    }

  
}