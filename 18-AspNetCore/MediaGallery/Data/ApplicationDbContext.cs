using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Data
{
    public class ApplicationDbContext : DbContext //IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
    }
}
