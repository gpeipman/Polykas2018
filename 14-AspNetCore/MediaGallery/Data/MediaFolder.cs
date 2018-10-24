using System.Collections.Generic;

namespace MediaGallery.Data
{
    public class MediaFolder : MediaItem 
    {
        public IList<MediaItem> Items { get; set; }
        public override string Thumbnail
        {
            get
            {
                return "folder.jpg";
            }
            set { }
        }

        public MediaFolder()
        {
            Items = new List<MediaItem>();
        }
    }
}
