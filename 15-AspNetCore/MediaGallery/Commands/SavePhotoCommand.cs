using MediaGallery.Models;

namespace MediaGallery.Commands
{
    public class SavePhotoCommand : CompositeCommand<PhotoEditModel>
    {
        public SavePhotoCommand(SavePhotoToStoreCommand storeCommand,
                                SavePhotoToDatabaseCommand dbCommand)
        {
            Add(storeCommand);
            Add(dbCommand);
        }
    }
}
