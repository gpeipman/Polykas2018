using System.Collections.Generic;
using System.Linq;
using MediaGallery.Data;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery
{
    public class GalleryContext
    { 

        private readonly ApplicationDbContext _dataContext;

        public MediaItem CurrentItem { get; set; }
        public string PageTitle { get; set; }

        public GalleryContext(ApplicationDbContext context)
        {
            _dataContext = context;
        }

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

        private IList<int> _currentFolderPath;

        public IList<int> GetCurrentFolderPath()
        {
            if (_currentFolderPath != null)
            {
                return _currentFolderPath;
            }

            var currentId = GetCurrentFolderId();
            if(currentId == null)
            {
                return new List<int>();
            }

            var folder = _dataContext.Folders
                                     .Include(f => f.ParentFolder)
                                     .FirstOrDefault(f => f.Id == currentId.Value);

            _currentFolderPath = new List<int>();
            
            while(folder.ParentFolder != null)
            {
                _currentFolderPath.Add(folder.Id);

                folder = folder.ParentFolder;
            }

            if(folder != null && folder.ParentFolder == null)
            {
                _currentFolderPath.Add(folder.Id);
            }

            return _currentFolderPath;
        }
    }
}
