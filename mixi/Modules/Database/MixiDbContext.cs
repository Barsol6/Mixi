using Microsoft.EntityFrameworkCore;
using mixi.Modules.Pdf;
using mixi.Modules.Users;

namespace mixi.Modules.Database;

public class MixiDbContext:DbContext
{
    public MixiDbContext(DbContextOptions<MixiDbContext> options)
        :base(options){ } 
    
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<PdfDocument> PdfDocuments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
        
        modelBuilder.Entity<PdfDocument>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(x => x.FilePath)
                .HasMaxLength(512);
            entity.Property(x => x.StorageStrategy)
                .HasDefaultValue(StorageStrategy.Database);
            entity.Property(x => x.Content)
                .IsRequired();
            entity.Property(x => x.FormData)
                .HasDefaultValue("{}");
            entity.HasIndex(x => x.Name)
                .HasDatabaseName("IX_PdfDocument_Name");
            entity.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("IX_PdfDocument_CreatedAt");
        });
        
        base.OnModelCreating(modelBuilder);
    }

  
}