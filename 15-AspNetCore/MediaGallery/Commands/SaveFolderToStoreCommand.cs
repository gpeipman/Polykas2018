using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaGallery.Data;
using MediaGallery.FileSystem;
using MediaGallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaGallery.Commands
{
    public class SaveFolderToStoreCommand : ICommand<EditFolderModel>
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<SaveFolderToStoreCommand> _logger;
        private readonly IHostingEnvironment _host;
        private readonly GalleryContext _galleryContext;
        private readonly IFileClient _fileClient;

        public SaveFolderToStoreCommand(ApplicationDbContext dataContext,
                                       GalleryContext galleryContext,
                                       IHostingEnvironment host,
                                       IFileClient fileClient,
                                       ILogger<SaveFolderToStoreCommand> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
            _galleryContext = galleryContext;
            _host = host;
            _fileClient = fileClient;
        }

        public bool Execute(EditFolderModel model)
        {
            var parentPath = GetParentPath(model);

            _fileClient.CreateFolder(parentPath, model.Title);

            return true;
        }

        private string GetParentPath(EditFolderModel model)
        {
            var parentPath = "";

            if (model.parentFolderId.HasValue)
            {
                var parentFolder = _dataContext.Folders.FirstOrDefault(f => f.Id == model.parentFolderId);

                parentPath = _galleryContext.GetFolderPath(model.parentFolderId.Value, null);
            }

            return parentPath;
        }

        public bool Rollback()
        {
            return true;
        }

        public List<string> Validate(EditFolderModel model)
        {
            var warnings = new List<string>();
            var parentPath = GetParentPath(model);

            if(_fileClient.HasFolder(Path.Combine(parentPath, model.Title)))
            {
                warnings.Add("Selle nimega kaust on juba olemas");
            }            

            return warnings;
        }
    }
}
