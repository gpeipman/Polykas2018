using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MediaFile>()
                .Property(b => b.Latitude)
                .HasColumnName("Video_Latitude");

            modelBuilder.Entity<MediaFile>()
                .Property(b => b.Longitude)
                .HasColumnName("Video_Longitude");
        }

        public DbSet<MediaItem> Items { get; set; }
        public DbSet<MediaFolder> Folders { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
