using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Pdf;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Database;

public class MixiDbContext:DbContext
{
    public MixiDbContext(DbContextOptions<MixiDbContext> options)
        :base(options){ } 
    
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<PdfDocument> PdfDocuments { get; set; }
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
            entity.HasMany(x => x.PdfDocuments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false);
        });
        
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
            entity.HasOne(x => x.User)
                .WithMany(x => x.PdfDocuments)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false);
            entity.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(255);
        });
        
        base.OnModelCreating(modelBuilder);
    }

  
}