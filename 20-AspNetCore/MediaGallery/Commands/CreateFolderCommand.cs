using MediaGallery.Models;

namespace MediaGallery.Commands
{
    public class CreateFolderCommand : CompositeCommand<EditFolderModel>
    {
        public CreateFolderCommand(SaveFolderToStoreCommand storeCommand,
                                   SaveFolderToDatabaseCommand dbCommand)
        {
            Add(storeCommand);
            Add(dbCommand);
        }
    }
}
