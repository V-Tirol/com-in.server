using Microsoft.EntityFrameworkCore;

namespace com_in.server.Models
{
    public class ForumContext : DbContext
    {
        public ForumContext(DbContextOptions<ForumContext> options): base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Media>().HasKey(c => c.Id);
            modelBuilder.Entity<Article>().HasKey(c => c.Id);
            modelBuilder.Entity<MediaType>().HasKey(c => c.Id);


            modelBuilder.Entity<Category>()
                .HasMany(a => a.Articles)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);

            
            modelBuilder.Entity<Media>()
                .HasOne(c => c.Category)
                .WithMany(m => m.Media)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Media>()
                .HasOne(t => t.Type)
                .WithMany(m => m.media)
                .HasForeignKey(t => t.TypeId);


            modelBuilder.Entity<Article>()
                .Property(a => a.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Category>()
                .Property(c => c.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Media>()
                .Property(m => m.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<MediaType>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaType> MediaType { get; set; }

        protected ForumContext() { }
    }
}
