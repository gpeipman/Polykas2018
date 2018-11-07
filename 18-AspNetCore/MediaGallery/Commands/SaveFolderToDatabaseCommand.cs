using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaGallery.Data;
using MediaGallery.Models;
using Microsoft.Extensions.Logging;

namespace MediaGallery.Commands
{
    public class SaveFolderToDatabaseCommand : ICommand<EditFolderModel>
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly ILogger<SaveFolderToDatabaseCommand> _logger;


        public SaveFolderToDatabaseCommand(ApplicationDbContext dataContext, ILogger<SaveFolderToDatabaseCommand> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public bool Execute(EditFolderModel model)
        {
            var folder = new MediaFolder();
            folder.Title = model.Title;

            if (model.parentFolderId.HasValue)
            {
                var parentFolder = _dataContext.Folders.FirstOrDefault(f => f.Id == model.parentFolderId);

                folder.ParentFolder = parentFolder;
                parentFolder.Items.Add(folder);
            }

            _dataContext.Items.Add(folder);
            _dataContext.SaveChanges();

            return true;
        }

        public bool Rollback()
        {
            return true;
        }

        public List<string> Validate(EditFolderModel parameter)
        {
            return new List<string>();
        }
    }
}
