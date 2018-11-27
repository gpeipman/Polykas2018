using System.Collections.Generic;

namespace MediaGallery.Data
{
    public abstract class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public abstract string Thumbnail { get; set; }
        
        public MediaFolder ParentFolder { get; set; }
        public IList<Comment> Comments { get; set; }

        public MediaItem()
        {
            Comments = new List<Comment>();
        }
    }
}
