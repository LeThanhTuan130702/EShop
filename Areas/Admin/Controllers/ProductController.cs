using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Data;
using ShopQuanAo.Models;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]

    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;


        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.products.Include(p => p.category).Include(p => p.color).Include(p => p.size);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
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

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name");
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Name");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price,Discout,Photo,SizeId,ColorId,IsFeatured,IsRecent,ImageUpLoad")] Product product)
        {
            if (ModelState.IsValid)
            {

                var name = await _context.products.FirstOrDefaultAsync(p => p.Name == product.Name);
                if(name!=null)
                {
                    TempData["error"] = "Đã có sản phẩm";
                    ModelState.AddModelError("", "đã có sản phẩm");
                    return View(product);
                }
                else
                {
                    if(product.ImageUpLoad!=null)
                    {
                        string uploaddsdir = Path.Combine(_webHost.WebRootPath,"img");
                        string imagename=Guid.NewGuid().ToString()+"_"+product.ImageUpLoad.FileName;
                        string filepath = Path.Combine(uploaddsdir, imagename);
                        FileStream fs = new FileStream(filepath,FileMode.Create);
                        await product.ImageUpLoad.CopyToAsync(fs);
                        fs.Close();
                        product.Photo = imagename;
                        try
                        {
                            _context.Add(product);
                            await _context.SaveChangesAsync();
                            TempData["success"] = "Tạo thành công";
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            // Xử lý lỗi khi thêm sản phẩm vào cơ sở dữ liệu không thành công
                            ModelState.AddModelError("", "Error saving changes: " + ex.Message);
                        }

                    }
                    
                }
                
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Name", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Name", product.SizeId);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int id)
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
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Name", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Name", product.SizeId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,/* [Bind("Id,Name,Description,CategoryId,Price,Discout,Photo,SizeId,ColorId,IsFeatured,IsRecent,ImageUpLoad")] */Product product)
        {
            if (ModelState.IsValid)
            {

                var name = await _context.products.FirstOrDefaultAsync(p => p.Name == product.Name);
                //var namenew = await _context.products.FirstOrDefaultAsync(p=>p.Id==id);
                if (name != null /*&& namenew.Name!=name.Name*/)
                {
                    TempData["error"] = "Đã có sản phẩm";
                    ModelState.AddModelError("", "đã có sản phẩm");
                    return View(product);
                }
                else
                {
                    if (product.ImageUpLoad != null)
                    {
                        string uploaddsdir = Path.Combine(_webHost.WebRootPath, "img");
                        string imagename = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                        string filepath = Path.Combine(uploaddsdir, imagename);
                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await product.ImageUpLoad.CopyToAsync(fs);
                        fs.Close();
                        product.Photo = imagename;
                        try
                        {
                            _context.Update(product);
                            await _context.SaveChangesAsync();
                            TempData["success"] = "Cập nhật thành công";
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            // Xử lý lỗi khi thêm sản phẩm vào cơ sở dữ liệu không thành công
                            ModelState.AddModelError("", "Error saving changes: " + ex.Message);
                        }

                    }

                }

            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.colors, "Id", "Name", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Name", product.SizeId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
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

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if(!string.Equals(product.Photo,"noname.jpg"))
            {
                string uploaddsdir = Path.Combine(_webHost.WebRootPath, "img");
                string oldfilepath = Path.Combine(uploaddsdir, product.Photo);
                if(System.IO.File.Exists(oldfilepath))
                {
                    System.IO.File.Delete(oldfilepath);
                }    

            }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xóa";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
