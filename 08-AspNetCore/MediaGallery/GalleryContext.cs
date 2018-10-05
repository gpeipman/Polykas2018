using MediaGallery.Data;

namespace MediaGallery
{
    public class GalleryContext
    {
        public MediaItem CurrentItem { get; set; }
        public string PageTitle { get; set; }

        public string GetFullPageTitle()
        {
            var title = "MediaGallery";

            if(!string.IsNullOrEmpty(PageTitle))
            {
                title = PageTitle + " - " + title;
            }

            return title;
        }

        public int? GetCurrentFolderId()
        {
            if(CurrentItem == null)
            {
                return null;
            }

            if(CurrentItem is MediaFolder)
            {
                return CurrentItem.Id;
            }

            if(CurrentItem.ParentFolder == null)
            {
                return null;
            }

            return CurrentItem.ParentFolder.Id;
        }
    }
}
