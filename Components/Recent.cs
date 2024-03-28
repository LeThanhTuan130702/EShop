using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Data;

namespace ShopQuanAo.Components
{
    public class Recent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Recent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.products.Where(p=>p.IsRecent==true).ToList());
        }
    }
}
