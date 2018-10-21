using System.Diagnostics;
using System.Linq;
using MediaGallery.Data;
using MediaGallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly GalleryContext _galleryContext;

        public HomeController(ApplicationDbContext dataContext,
                              GalleryContext galleryContext)
        {
            _dataContext = dataContext;
            _galleryContext = galleryContext;
        }

        public IActionResult Index()
        {
            var model = new FrontPageModel();
            model.NewPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();
            model.PopularPhotos = _dataContext.Photos.Cast<MediaItem>().ToList();

            var path = _galleryContext.GetFolderPath(8, "lill.jpg");

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
        public IActionResult CreateFolder(EditFolderModel model)
        {
            var folder = new MediaFolder();
            folder.Title = model.Title;

            if(model.parentFolderId.HasValue)
            {
                var parentFolder = _dataContext.Folders.FirstOrDefault(f => f.Id == model.parentFolderId);

                folder.ParentFolder = parentFolder;
                parentFolder.Items.Add(folder);
            }

            _dataContext.Items.Add(folder);
            _dataContext.SaveChanges();

            return RedirectToAction("Details", new { id = folder.Id });
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
    }
}
