using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediaGallery.Models;
using MediaGallery.Data;

namespace MediaGallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dataContext;

        public HomeController(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            
            if(_dataContext.Folders.Count() == 0)
            {
                var folder = new MediaFolder();
                folder.Title = "Lilled";

                var photo = new Photo();
                photo.Title = "Lill 1";
                photo.FileName = "lill1.jpg";
                folder.Items.Add(photo);

                photo = new Photo();
                photo.Title = "Lill 2";
                photo.FileName = "lill2.jpg";
                folder.Items.Add(photo);

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
            model.NewPhotos = _dataContext.Items.ToList();
            model.PopularPhotos = _dataContext.Items.ToList();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var item = _dataContext.Items.FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                return NotFound();
            }

            return View("Picture", item);
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
