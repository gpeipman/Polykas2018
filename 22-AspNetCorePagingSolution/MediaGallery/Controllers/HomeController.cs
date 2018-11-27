using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp.Processing;
using MediaGallery.Commands;
using MediaGallery.Data;
using MediaGallery.FileSystem;
using MediaGallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using ExifLibrary;
using Microsoft.AspNetCore.Authorization;

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

        public IActionResult Index(int page = 1)
        {
            page = Math.Max(1, page);

            var inRole = User.IsInRole("Admin");
            var model = new FrontPageModel();
            model.AllPhotos = _dataContext.Photos
                                    .Cast<MediaItem>()
                                    .GetPaged(page, 5);

            model.NewPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();
            model.PopularPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var item = LoadMediaItem(id); 
                                   
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult UploadFile(int? parentFolder)
        {
            ViewBag.ParentFolderId = parentFolder;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

                var img = ImageFile.FromStream(file.OpenReadStream());
                var latObject = (GPSLatitudeLongitude)img.Properties.FirstOrDefault(p => p.Name == "GPSLatitude");
                var lonObject = (GPSLatitudeLongitude)img.Properties.FirstOrDefault(p => p.Name == "GPSLongitude");
                if (latObject != null && lonObject != null)
                {
                    model.Latitude = latObject.ToFloat();
                    model.Longitude = lonObject.ToFloat();
                }

                list.AddRange(savePhotoCommand.Validate(model));

                savePhotoCommand.Execute(model);
            }

            ViewBag.Messages = list;

            return View();
        }

        [HttpGet]
        public void GetFile(int id)
        {
            var item = _dataContext.Items
                                   .Include(p => p.ParentFolder)
                                   .FirstOrDefault(i => i.Id == id);

            var path = "";
            var fileName = "";

            if (item is MediaFolder)
            {
                fileName = "folder.jpg";
                path = fileName;
            }
            else
            {
                var fileItem = (Photo)item;
                var folder = fileItem.ParentFolder;
                path = fileItem.FileName;
                if (folder != null)
                {
                    path = _galleryContext.GetFolderPath(folder.Id, fileItem.FileName);
                }
                fileName = fileItem.FileName;
            }

            Response.Clear();
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFile(int id)
        {
            var file = _dataContext.Photos
                                   .Include(p => p.ParentFolder)
                                   .FirstOrDefault(p => p.Id == id);

            var parentId = file.ParentFolder?.Id;

            _dataContext.Photos.Remove(file);
            _dataContext.SaveChanges();

            if(parentId == null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = parentId });
        }

        public void GetFileWithEffect(int id, string effect)
        {
            var item = _dataContext.Items
                                   .Include(p => p.ParentFolder)
                                   .FirstOrDefault(i => i.Id == id);

            var fileItem = (Photo)item;
            var folder = fileItem.ParentFolder;
            var path = fileItem.FileName;
            if (folder != null)
            {
                path = _galleryContext.GetFolderPath(folder.Id, fileItem.FileName);
            }
            var fileName = fileItem.FileName;

            Response.Clear();
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);

            try
            {
                IImageFormat format;
                using (var fileStream = _fileClient.GetFile(path))
                using(var image = Image.Load(fileStream, out format))
                {
                    Image<Rgba32> imageWithEffect;

                    if(effect == "BlackWhite")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.BlackWhite());
                    }
                    else if(effect == "OilPaint")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.OilPaint());
                    }
                    else if (effect == "Sepia")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.Sepia());
                    }
                    else if (effect == "Blur")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.GaussianBlur());
                    }
                    else if (effect == "Sharpen")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.GaussianSharpen());
                    }
                    else if (effect == "Glow")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.Glow());
                    }
                    else if (effect == "Invert")
                    {
                        imageWithEffect = image.Clone(ctx => ctx.Invert());
                    }
                    else
                    {
                        imageWithEffect = image;
                    }

                    imageWithEffect.Save(Response.Body, format);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult SaveComment(Comment comment, int mediaItemId)
        {
            if (!ModelState.IsValid)
            {
                var item = LoadMediaItem(mediaItemId);

                return View("Picture", item);
            }

            comment.MediaItem = _dataContext.Items.FirstOrDefault(i => i.Id == mediaItemId);
            comment.Time = DateTime.Now;
            comment.User = _dataContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            _dataContext.Comments.Add(comment);
            _dataContext.SaveChanges();

            return RedirectToAction("Details", new { id = mediaItemId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            var comment = _dataContext.Comments
                                      .Include(c => c.MediaItem)
                                      .FirstOrDefault(c => c.Id == id);

            _dataContext.Comments.Remove(comment);
            _dataContext.SaveChanges();

            return RedirectToAction("Details", new { id = comment.MediaItem.Id });
        }

        [NonAction]
        private MediaItem LoadMediaItem(int id)
        {
            return _dataContext.Items
                                .Include(i => i.ParentFolder)
                                .Include(i => ((MediaFolder)i).Items)
                                .Include(i => i.Comments)
                                .ThenInclude(c => c.User)
                                .FirstOrDefault(i => i.Id == id);
        }
    }
}
