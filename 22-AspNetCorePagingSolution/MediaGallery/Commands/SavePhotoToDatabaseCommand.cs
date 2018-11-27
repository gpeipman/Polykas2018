using System.Collections.Generic;
using System.Linq;
using MediaGallery.Data;
using MediaGallery.Models;
using Microsoft.Extensions.Logging;

namespace MediaGallery.Commands
{
    public class SavePhotoToDatabaseCommand : ICommand<PhotoEditModel>
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<SavePhotoToDatabaseCommand> _logger;

        public SavePhotoToDatabaseCommand(ApplicationDbContext dataContext, ILogger<SavePhotoToDatabaseCommand> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public bool Execute(PhotoEditModel model)
        {
            MediaFolder folder = null;

            if (model.ParentFolderId.HasValue)
            {
                folder = folder = _dataContext.Folders.First(f => f.Id == model.ParentFolderId.Value);
            }

            var photo = new Photo();
            photo.FileName = model.FileName;
            photo.Thumbnail = model.FileName;
            photo.Latitude = model.Latitude;
            photo.Longitude = model.Longitude;

            if (folder != null)
            {
                folder.Items.Add(photo);
                photo.ParentFolder = folder;
            }

            _dataContext.Photos.Add(photo);
            _dataContext.SaveChanges();

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
