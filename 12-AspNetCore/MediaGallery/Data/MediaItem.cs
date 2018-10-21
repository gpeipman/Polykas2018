using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaGallery.Data
{
    public abstract class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public abstract string Thumbnail { get; set; }
        
        public MediaFolder ParentFolder { get; set; }
    }
}
