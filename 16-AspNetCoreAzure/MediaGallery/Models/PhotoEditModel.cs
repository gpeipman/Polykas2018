using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MediaGallery.Models
{
    public class PhotoEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Title { get; set; }

        public string Thumbnail { get; set; }
        public string FileName { get; set; }
        public int? ParentFolderId { get; set; }
        public IFormFile File { get; set; }
    }
}
