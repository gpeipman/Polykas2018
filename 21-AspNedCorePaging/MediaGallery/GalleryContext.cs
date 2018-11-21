using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaGallery.Data;
using MediaGallery.FileSystem;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery
{
    public class GalleryContext
    { 
        private readonly ApplicationDbContext _dataContext;
        private readonly IFileClient _fileClient;

        public MediaItem CurrentItem { get; set; }
        public string PageTitle { get; set; }
        public bool DownloadFiles { get; } = true;

        public GalleryContext(ApplicationDbContext context, IFileClient fileClient)
        {
            _dataContext = context;
            _fileClient = fileClient;
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

        public string GetFolderPath(int id, string fileName)
        {
            var folder = _dataContext.Folders
                                     .Include(f => f.ParentFolder)
                                     .FirstOrDefault(f => f.Id == id);

            var folderPath = new List<string>();
            folderPath.Add(fileName);

            while (folder.ParentFolder != null)
            {
                folderPath.Add(folder.Title);

                folder = folder.ParentFolder;
            }

            if (folder != null && folder.ParentFolder == null)
            {
                folderPath.Add(folder.Title);
            }

            return string.Join('/', folderPath.Reverse<string>().ToArray());     
        }

        public string GetItemUrl(MediaItem item)
        {
            if(DownloadFiles)
            {
                return "~/Home/GetFile/" + item.Id;
            }

            var folderPath = "";

            if (item is MediaFolder)
            {
                folderPath = item.Thumbnail;
            }
            else
            {
                var folderId = GetCurrentFolderId();
                if (folderId.HasValue)
                {
                    folderPath = GetFolderPath(folderId.Value, null);
                }

                folderPath = Path.Combine(folderPath, item.Thumbnail);
            }

            return _fileClient.GetFileUrl(folderPath);
        }
    }
}
