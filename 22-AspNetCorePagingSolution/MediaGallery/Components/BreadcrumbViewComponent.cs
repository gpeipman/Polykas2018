using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaGallery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Components
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly GalleryContext _galleryContext;

        public BreadcrumbViewComponent(ApplicationDbContext dataContext,
                                       GalleryContext galleryContext)
        {
            _dataContext = dataContext;
            _galleryContext = galleryContext;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var items = new Dictionary<int, string>();

            if(_galleryContext.CurrentItem != null)
            {
                var item = GetWithParent(_galleryContext.CurrentItem.Id);
                if (item.ParentFolder != null)
                {
                    var parent = GetWithParent(item.ParentFolder.Id);

                    while (parent != null)
                    {
                        items.Add(parent.Id, parent.Title);

                        if (parent.ParentFolder == null)
                        {
                            break;
                        }

                        parent = GetWithParent(parent.ParentFolder.Id);
                    }
                }
            }

            items = items.Reverse().ToDictionary(k => k.Key, v => v.Value);

            return View("Index", items);
        }

        private MediaItem GetWithParent(int id)
        {
            return _dataContext.Items
                               .Include(f => f.ParentFolder)
                               .FirstOrDefault(f => f.Id == id);
        }
    }
}
