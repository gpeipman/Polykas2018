using System.IO;

namespace MediaGallery.FileSystem
{
    public interface IFileClient
    {
        void CreateFolder(string parentPath, string folderName);
        void UploadPhoto(string path, Stream photo);
        bool HasFolder(string folderPath);
        string GetFileUrl(string path);
        Stream GetFile(string path);
    }
}
