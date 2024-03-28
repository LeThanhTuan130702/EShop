using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Data;

namespace ShopQuanAo.Components
{
    
    
        public class ImageBar : ViewComponent
        {
            private readonly ApplicationDbContext _context;

            public ImageBar(ApplicationDbContext context)
            {
                _context = context;
            }
            public IViewComponentResult Invoke()
            {
                return View(_context.categories.ToList());
            }
        }
    
}
