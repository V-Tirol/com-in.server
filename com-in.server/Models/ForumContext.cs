using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
            modelBuilder.Entity<MediaType>().HasKey(c => c.Id);

            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Department>().HasKey(c => c.Id);
            modelBuilder.Entity<AnnouncementCategories>().HasKey(c => c.id);
            modelBuilder.Entity<Announcements>().HasKey(c => c.Id);

            modelBuilder.Entity<Login>().HasKey(c => c.Id);
            modelBuilder.Entity<Student>().HasKey(c => c.Id);
            modelBuilder.Entity<Faculty>().HasKey(c => c.Id);
            modelBuilder.Entity<Alumni>().HasKey(c => c.Id);
            modelBuilder.Entity<Organization>().HasKey(c => c.Id);
            modelBuilder.Entity<Admin>().HasKey(c => c.Id);



            

            modelBuilder.Entity<Media>()
                .HasOne(t => t.Type)
                .WithMany(m => m.media)
                .HasForeignKey(t => t.TypeId);



            modelBuilder.Entity<Category>()
                .Property(c => c.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<MediaType>()
            .Property(t => t.isActive)
                .HasDefaultValue(true);


            modelBuilder.Entity<Login>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Student>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Faculty>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Alumni>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Organization>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Admin>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Course>()
                .Property(t => t.isDeleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<Department>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<TextMedia>()
                .Property (t => t.isDeleted)
                .HasDefaultValue (false);
            modelBuilder.Entity<AnnouncementCategories>()
                .Property(t => t.isActive)
                .HasDefaultValue(true);


        }

        public DbSet<TextMedia> TextMedia { get; set; }
        public DbSet<MultiMedia> MultiMedia { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaType> MediaType { get; set; }
        public DbSet<AnnouncementCategories> AnnouncementCategories { get; set; }
        public DbSet<Announcements> Announcements { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Department { get; set; }

        /* USERS AND ADMIN */
        public DbSet<Login> Logins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Alumni> Alumni { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Admin> Admins { get; set; }


        protected ForumContext() { }
    }
}
