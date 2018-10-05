using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaGallery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MediaGallery.Components
{
    public class GalleryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dataContext;

        public GalleryMenuViewComponent(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var folders = _dataContext.Folders
                                      .OrderBy(f => f.Title)
                                      .ToList();

            return View("Index", folders);
        }
    }
}
