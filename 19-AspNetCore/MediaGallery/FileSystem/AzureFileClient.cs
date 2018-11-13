using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MediaGallery.FileSystem
{
    public class AzureFileClient : IFileClient
    {
        private const string ConnectionString = "";

        private CloudStorageAccount _account;
        private CloudBlobClient _blobClient;

        public AzureFileClient()
        {
            _account = CloudStorageAccount.Parse(ConnectionString);
            _blobClient = _account.CreateCloudBlobClient();
        }

        public void CreateFolder(string parentPath, string folderName)
        {
        }

        public Stream GetFile(string path)
        {
            var container = _blobClient.GetContainerReference("gallery");
            var blob = container.GetBlockBlobReference(path.ToLower());

            var mem = new MemoryStream();
            var task = blob.DownloadToStreamAsync(mem);
            task.Wait();

            mem.Seek(0, SeekOrigin.Begin);

            return mem;
        }

        public string GetFileUrl(string path)
        {
            var container = _blobClient.GetContainerReference("gallery");
            var blob = container.GetBlockBlobReference(path.ToLower());

            return blob.Uri.AbsoluteUri;
        }

        public bool HasFolder(string folderPath)
        {
            return false;
        }

        public void UploadPhoto(string path, Stream photo)
        {
            var container = _blobClient.GetContainerReference("gallery");
            var blob = container.GetBlockBlobReference(path.ToLower());
            blob.UploadFromStreamAsync(photo).Wait();
        }
    }
}
