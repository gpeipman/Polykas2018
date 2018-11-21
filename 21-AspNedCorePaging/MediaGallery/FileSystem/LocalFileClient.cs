using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MediaGallery.FileSystem
{
    public class LocalFileClient : IFileClient
    {
        private readonly IHostingEnvironment _host;

        public LocalFileClient(IHostingEnvironment host)
        {
            _host = host;
        }

        public void CreateFolder(string parentPath, string folderName)
        {
            var path = Path.Combine(_host.WebRootPath, "gallery", parentPath, folderName);
            Directory.CreateDirectory(path);
        }

        public Stream GetFile(string path)
        {
            var filePath = Path.Combine(_host.WebRootPath, "gallery", path);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public string GetFileUrl(string path)
        {
            return "~/" + Path.Combine("gallery", path);
        }

        public bool HasFolder(string folderPath)
        {
            var path = Path.Combine(_host.WebRootPath, "gallery", folderPath);

            return Directory.Exists(path);
        }

        public void UploadPhoto(string path, Stream photo)
        {
            path = Path.Combine(_host.WebRootPath, "gallery", path);

            using (var localFile = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
            {
                photo.CopyTo(localFile);
            }
        }
    }
}
