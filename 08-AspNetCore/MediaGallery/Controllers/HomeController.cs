using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediaGallery.Models;
using MediaGallery.Data;
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
            if(_dataContext.Folders.Count() == 0)
            {
                var folder = new MediaFolder();
                folder.Title = "Lilled";                

                var photo = new Photo();
                photo.Title = "Lill 2";
                photo.FileName = "lill2.jpg";
                folder.Items.Add(photo);

                _dataContext.Folders.Add(folder);

                var subFolder = new MediaFolder();
                subFolder.Title = "Roosid";
                subFolder.ParentFolder = folder;                

                photo = new Photo();
                photo.Title = "Lill 1";
                photo.FileName = "lill1.jpg";
                photo.ParentFolder = subFolder;
                subFolder.Items.Add(photo);
                folder.Items.Add(subFolder);

                _dataContext.Folders.Add(subFolder);

                folder = new MediaFolder();
                folder.Title = "Majad";
                _dataContext.Folders.Add(folder);

                folder = new MediaFolder();
                folder.Title = "Söögid";
                _dataContext.Folders.Add(folder);

                photo = new Photo();
                photo.Title = "Pealkiri 1";
                photo.FileName = "pilt.jpg";
                _dataContext.Photos.Add(photo);

                photo = new Photo();
                photo.Title = "Pealkiri 2";
                photo.FileName = "pilt2.jpg";
                _dataContext.Photos.Add(photo);

                photo = new Photo();
                photo.Title = "Pealkiri 3";
                photo.FileName = "pilt3.jpg";
                _dataContext.Photos.Add(photo);

                _dataContext.SaveChanges();
            }

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
