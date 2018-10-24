using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaGallery.Data;
using MediaGallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Controllers
{
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _dataContext;

        public ApiController(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> List(int? parentId)
        {
            var query = (IQueryable<MediaItem>)_dataContext.Items.Include(i => i.ParentFolder);                                    

            if(parentId.HasValue)
            {
                query = query.Where(i => i.ParentFolder.Id == parentId.Value);
            }
            else
            {
                query = query.Where(i => i.ParentFolder == null);
            }

            var items = await query.OrderBy(i => i.Title).ToListAsync();
            var models = new List<MediaItemApiModel>();

            foreach(var item in items)
            {
                var model = new MediaItemApiModel();
                model.Id = item.Id;
                model.Thumbnail = item.Thumbnail;
                model.Title = item.Title;
                model.MediaItemType = item.GetType().Name;

                if(item is MediaFile)
                {
                    model.FileName = ((MediaFile)item).FileName;
                }

                models.Add(model);
            }
                                   

            return Json(models);
        }
    }
}