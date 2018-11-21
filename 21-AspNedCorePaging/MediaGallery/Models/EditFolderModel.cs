using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaGallery.Models
{
    public class EditFolderModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? parentFolderId { get; set; }
    }
}
