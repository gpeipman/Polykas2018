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
            if(_dataContext.Photos.Count() == 0)
            {
                var photo = new Photo();
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
            model.NewPhotos = _dataContext.Photos.Cast<MediaFile>().ToList();
            model.PopularPhotos = _dataContext.Photos.Cast<MediaFile>().ToList();

            return View(model);
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
