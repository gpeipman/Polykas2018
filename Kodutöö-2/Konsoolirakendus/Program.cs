using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleDriveConsole
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
        static string ApplicationName = "MinuProjekt"; // KORREKTNE PROJEKTI NIMI PEAB OLEMA


        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "mingiasi@gmail.com", // OMA GMAIL AADRESS SIIA!!!
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            // FAILI UPLOAD
            var body = new Google.Apis.Drive.v3.Data.File();
            body.Name = Path.GetFileName("blah.txt");

            byte[] byteArray = System.IO.File.ReadAllBytes("blah.txt");
            System.IO.MemoryStream uploadStream = new System.IO.MemoryStream(byteArray);
            try
            {
                FilesResource.CreateMediaUpload request = service.Files.Create(body, uploadStream, "text/plain");
                var y = request.Upload(); 
                var x = request.ResponseBody; // SIIA BREAKPOINT KUI VIGU ON JA KONTROLLI y VÄÄRTUST
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            // FAILIDE LOEND
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
        }
    }
}
