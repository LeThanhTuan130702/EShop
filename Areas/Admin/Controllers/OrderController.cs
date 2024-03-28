using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Data;
using Microsoft.AspNetCore.Identity;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webhost;

        public OrderController(ApplicationDbContext context, IWebHostEnvironment webhost)
        {
            _context = context;
            _webhost = webhost;
        }

        public async Task< IActionResult>Index()
        {

            return View(await _context.orders.OrderByDescending(p=>p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var orderView= await _context.orderDetails.Where(o => o.OrderCode == ordercode).ToListAsync();
            return View(orderView);
        }
        public async Task<IActionResult> Delete(string? ordercode)
        {
           

            var order = await _context.orders
                .FirstOrDefaultAsync(m => m.Order_Code == ordercode);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string ordercode)
        {
            var order = await _context.orders
              .FirstOrDefaultAsync(m => m.Order_Code == ordercode);

            if (order == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }


            _context.orders.Remove(order);
            _context.SaveChanges();
            TempData["error"] = "đã xóa Order ";
            //var IsOrderDetail = await _context.orderDetails
            //  .FirstOrDefaultAsync(m => m.OrderCode == ordercode);
            //if (IsOrderDetail == null)
            //{
            //    //await DeleteConfirmed(ordercode);

            //}
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteView(string? ordercode)
        {


            var orderdetail = await _context.orderDetails
                .FirstOrDefaultAsync(m => m.OrderCode == ordercode);
            if (orderdetail == null)
            {
                return NotFound();
            }

            return View(orderdetail);
        }
        [HttpPost, ActionName("DeleteView")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedView(string ordercode)
        {
            var orderdetail = await _context.orderDetails
              .FirstOrDefaultAsync(m => m.OrderCode == ordercode);

            if (orderdetail == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }


            _context.orderDetails.Remove(orderdetail);
            _context.SaveChanges();
            TempData["error"] = "đã xóa Order ";
            var IsOrderDetail= await _context.orderDetails
              .FirstOrDefaultAsync(m => m.OrderCode == ordercode);
            if(IsOrderDetail==null)
            {
               //await DeleteConfirmed(ordercode);

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
