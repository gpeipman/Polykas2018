using System.ComponentModel.DataAnnotations;

namespace MediaGallery.Models
{
    public class PhotoEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Title { get; set; }
    }
}
