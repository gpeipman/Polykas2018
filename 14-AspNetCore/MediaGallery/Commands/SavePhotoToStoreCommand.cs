using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaGallery.Data;
using MediaGallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaGallery.Commands
{
    public class SavePhotoToStoreCommand : ICommand<PhotoEditModel>
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<SavePhotoToDatabaseCommand> _logger;
        private readonly IHostingEnvironment _host;
        private readonly GalleryContext _context;

        public SavePhotoToStoreCommand(ApplicationDbContext dataContext, 
                                       GalleryContext galleryContext,
                                       IHostingEnvironment host,
                                       ILogger<SavePhotoToDatabaseCommand> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
            _context = galleryContext;
            _host = host;
        }

        public bool Execute(PhotoEditModel model)
        {
            var parentPath = "";
            MediaFolder folder = null;

            if (model.ParentFolderId.HasValue)
            {
                folder = _dataContext.Folders.First(f => f.Id == model.ParentFolderId.Value);
                parentPath = _context.GetFolderPath(model.ParentFolderId.Value, null);
            }

            var fileName = Path.GetFileName(model.FileName);
            var filePath = Path.Combine(_host.WebRootPath, "gallery", parentPath, fileName);

            using (var localFile = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                model.File.CopyTo(localFile);
            }

            return true;
        }

        public bool Rollback()
        {
            return true;
        }

        public List<string> Validate(PhotoEditModel parameter)
        {
            return new List<string>();
        }
    }
}
