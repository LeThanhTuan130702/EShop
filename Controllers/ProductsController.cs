using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Data;
using ShopQuanAo.Models;
using ShopQuanAo.Models.ViewFolder;
using static ShopQuanAo.Controllers.ProductsController;

namespace ShopQuanAo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int pageSize = 9;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public  IActionResult Index()
        {
            var product =_context.products.ToList();
            return View(product);


           

        }
        public async Task<IActionResult> Search(string Name)
        {
            var product = await _context.products.Where(p => p.Name.Contains(Name)).ToListAsync();
            return View(product);


           

        }
        //public async Task<IActionResult> ProductByCate(int CateId)
        //{

        //    var applicationDbContext = _context.products.Where(p => p.CategoryId==CateId).Include(p=>p.category).Include(p => p.color).Include(p => p.size);
        //    return View("Index",await applicationDbContext.ToListAsync());
        //}

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.category)
                .Include(p => p.color)
                .Include(p => p.size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id");
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Id");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price,Discout,Photo,SizeId,ColorId,IsFeatured,IsRecent")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Id", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id", product.SizeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Id", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id", product.SizeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Price,Discout,Photo,SizeId,ColorId,IsFeatured,IsRecent")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Id", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id", product.SizeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.category)
                .Include(p => p.color)
                .Include(p => p.size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
