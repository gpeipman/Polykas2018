using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaGallery.Data
{
    public abstract class MediaFile : MediaItem
    {
        public string FileName { get; set; }
    }
}
