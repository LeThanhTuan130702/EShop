using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Data;
using ShopQuanAo.Models;

namespace ShopQuanAo.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int Id)
        {
              return _context.categories != null ? 
                          View(await _context.products.Where(p=>p.CategoryId==Id).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.categories'  is null.");
        }

      
      
    }
}
