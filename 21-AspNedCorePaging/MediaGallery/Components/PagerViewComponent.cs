using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MediaGallery.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return View("Index", result);
        }
    }
}
