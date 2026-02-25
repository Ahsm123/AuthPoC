using AuthPoC.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthPoC.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.Username).HasMaxLength(100).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Role).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Article>(e =>
        {
            e.HasKey(a => a.ArticleId);
            e.Property(a => a.Title).HasMaxLength(200).IsRequired();
            e.Property(a => a.Content).IsRequired();
            e.HasOne(a => a.Author)
             .WithMany(u => u.Articles)
             .HasForeignKey(a => a.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.HasKey(c => c.CommentId);
            e.Property(c => c.Content).IsRequired();
            e.HasOne(c => c.Article)
             .WithMany(a => a.Comments)
             .HasForeignKey(c => c.ArticleId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(c => c.User)
             .WithMany(u => u.Comments)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
