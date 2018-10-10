using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaGallery.Data
{
    public class MediaFolder : MediaItem 
    {
        public IList<MediaItem> Items { get; set; }
        public override string Thumbnail
        {
            get
            {
                //if (Items.Count() > 0)
                //{
                //    return Items[0].Thumbnail;
                //}

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
