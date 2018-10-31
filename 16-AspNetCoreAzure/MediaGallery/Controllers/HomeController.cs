using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MediaGallery.Commands;
using MediaGallery.Data;
using MediaGallery.FileSystem;
using MediaGallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediaGallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly GalleryContext _galleryContext;
        private readonly IHostingEnvironment _host;
        private readonly IFileClient _fileClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dataContext,
                              IHostingEnvironment host,
                              IFileClient fileClient,
                              ILogger<HomeController> logger,
                              GalleryContext galleryContext)
        {
            _dataContext = dataContext;
            _galleryContext = galleryContext;
            _host = host;
            _fileClient = fileClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogWarning("asd");
            var model = new FrontPageModel();
            model.NewPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();
            model.PopularPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var item = _dataContext.Items
                                   .Include(i => i.ParentFolder)
                                   .Include(i => ((MediaFolder)i).Items)
                                   .FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _galleryContext.PageTitle = item.Title;
            _galleryContext.CurrentItem = item;

            if(item is MediaFolder)
            {
                return View("Folder", item);
            }

            return View("Picture", item);
        }

        public IActionResult CreateFolder(int? parentFolder)
        {
            var model = new EditFolderModel();
            model.parentFolderId = parentFolder;

            return View(model);
        }
    
        [HttpPost]
        public IActionResult CreateFolder(EditFolderModel model, [FromServices]CreateFolderCommand createFolderCommand)
        {
            var messages = createFolderCommand.Validate(model);
            if(messages.Count > 0)
            {
                ModelState.AddModelError("_FORM", messages[0]);
                return View(model);
            }

            createFolderCommand.Execute(model);

            return Redirect(Url.Content("~/"));
        } 

        public IActionResult Edit(int id)
        {
            var item = _dataContext.Photos.FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                return NotFound();
            }

            var model = new PhotoEditModel();
            model.Title = item.Title;
            model.Id = item.Id;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PhotoEditModel model)
        {
            var item = _dataContext.Photos.FirstOrDefault(i => i.Id == model.Id);
            if(item == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            item.Title = model.Title;

            _dataContext.SaveChanges();

            return RedirectToAction("Details", new { id = model.Id });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult UploadFile(int? parentFolder)
        {
            ViewBag.ParentFolderId = parentFolder;

            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IList<IFormFile> files, int? parentFolder, [FromServices]SavePhotoCommand savePhotoCommand)
        {
            var list = new List<string>();

            foreach(var file in files)
            {
                var model = new PhotoEditModel();
                model.FileName = Path.GetFileName(file.FileName);
                model.Thumbnail = Path.GetFileName(file.FileName);
                model.ParentFolderId = parentFolder;
                model.File = file;

                list.AddRange(savePhotoCommand.Validate(model));

                savePhotoCommand.Execute(model);
            }

            ViewBag.Messages = list;

            return View();
        }

        [HttpGet]
        public void GetFile(int id)
        {
            var item = _dataContext.Photos
                                   .Include(p => p.ParentFolder)
                                   .FirstOrDefault(i => i.Id == id);

            var folder = item.ParentFolder;
            var path = item.FileName;
            if (folder != null)
            {
                path = _galleryContext.GetFolderPath(folder.Id, item.FileName);
            }

            Response.Clear();
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + item.FileName);

            try
            {
                using (var fileStream = _fileClient.GetFile(path))
                {
                    fileStream.CopyTo(Response.Body);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }
    }
}
