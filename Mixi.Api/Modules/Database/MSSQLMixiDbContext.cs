using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Users;
using Mixi.Api.Modules.Music;

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


        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Description);
            entity.Property(x => x.ImageUrl);
            entity.Property(x => x.Name)
                .IsRequired();
            entity.Property(x => x.UserId)
                .IsRequired();
            entity.HasMany(x => x.PlaylistItems)
                .WithOne(x => x.Playlist)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlaylistItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Album);
            entity.Property(x => x.Artist);
            entity.Property(x => x.Duration);
            entity.Property(x => x.Title).IsRequired();;
            entity.Property(x => x.SourceIdentifier).IsRequired();
            entity.Property(x => x.SourceType).IsRequired();
            
        });
        

        
        base.OnModelCreating(modelBuilder);
    }

  
}