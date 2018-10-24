using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaGallery.Models
{
    public class MediaItemApiModel
    {
        public string Thumbnail { get; set; }
        public string FileName { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string MediaItemType { get; set; }
    }
}
