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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webhost;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment webhost)
        {
            _context = context;
            _webhost = webhost;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
              return _context.categories != null ? 
                          View(await _context.categories.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.categories'  is null.");
        }

        // GET: Admin/Categories/Details/5
        

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Id,Name,Photo,CategoryOrders,ImageUpLoadCate")]*/ Category category)
        {
            if (ModelState.IsValid)
            {

                var name = await _context.categories.FirstOrDefaultAsync(p => p.Name == category.Name);
                if (name != null)
                {
                    TempData["error"] = "Đã có sản phẩm";
                    ModelState.AddModelError("", "đã có sản phẩm");
                    return View(category);
                }
                else
                {
                    if (category.ImageUpLoadCate != null)
                    {
                        string uploaddsdir = Path.Combine(_webhost.WebRootPath, "img");
                        string imagename = Guid.NewGuid().ToString() + "_" + category.ImageUpLoadCate.FileName;
                        string filepath = Path.Combine(uploaddsdir, imagename);
                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await category.ImageUpLoadCate.CopyToAsync(fs);
                        fs.Close();
                        category.Photo = imagename;
                        try
                        {
                            _context.Add(category);
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
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,/* [Bind("Id,Name,Photo,CategoryOrders,ImageUpLoadCate")]*/ Category category)
        {
            if (ModelState.IsValid)
            {

                var name = await _context.categories.FirstOrDefaultAsync(p => p.Name == category.Name);
                //var namenew = await _context.products.FirstOrDefaultAsync(p=>p.Id==id);
                if (name != null /*&& namenew.Name!=name.Name*/)
                {
                    TempData["error"] = "Đã có sản phẩm";
                    ModelState.AddModelError("", "đã có sản phẩm");
                    return View(category);
                }
                else
                {
                    if (category.ImageUpLoadCate != null)
                    {
                        string uploaddsdir = Path.Combine(_webhost.WebRootPath, "img");
                        string imagename = Guid.NewGuid().ToString() + "_" + category.ImageUpLoadCate.FileName;
                        string filepath = Path.Combine(uploaddsdir, imagename);
                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await category.ImageUpLoadCate.CopyToAsync(fs);
                        fs.Close();
                        category.Photo = imagename;
                        try
                        {
                            _context.Update(category);
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var categories = await _context.categories.FindAsync(id);
            if (!string.Equals(categories.Photo, "noname.jpg"))
            {
                string uploaddsdir = Path.Combine(_webhost.WebRootPath, "img");
                string oldfilepath = Path.Combine(uploaddsdir, categories.Photo);
                if (System.IO.File.Exists(oldfilepath))
                {
                    System.IO.File.Delete(oldfilepath);
                }

            }
            _context.categories.Remove(categories);
            await _context.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xóa";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
