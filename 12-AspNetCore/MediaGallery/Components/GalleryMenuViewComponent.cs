using System.Linq;
using System.Threading.Tasks;
using MediaGallery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace MediaGallery.Components
{
    public class GalleryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dataContext;

        public GalleryMenuViewComponent(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ViewViewComponentResult> InvokeAsync(MediaFolder parentFolder = null)
        {
            _dataContext.Folders.ToList();

            IQueryable<MediaFolder> folders = _dataContext.Folders;
            if(parentFolder == null)
            {
                folders = folders.Where(f => f.ParentFolder == null);
            }
            else
            {
                folders = folders.Where(f => f.ParentFolder.Id == parentFolder.Id);
            }

            folders = folders.OrderBy(f => f.Title);

            return View("Index", folders.ToList());
        }
    }
}
