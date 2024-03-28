using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Data;

namespace ShopQuanAo.Components
{
    public class Featured : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Featured(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.products.Where(p=>p.IsFeatured==true).ToList());
        }
    }
}
