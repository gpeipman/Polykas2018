using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MediaGallery.Data
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 4)]
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public IdentityUser User { get; set; }
        public MediaItem MediaItem { get; set; }
    }
}
