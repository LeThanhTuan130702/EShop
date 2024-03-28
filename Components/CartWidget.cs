using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Data;
using ShopQuanAo.Infrastructure;
using ShopQuanAo.Models;

namespace ShopQuanAo.Components
{
    public class CartWidget : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CartWidget(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<CartModel>? cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");
			CartItemViewModel? cartItemViewModel=new CartItemViewModel();

			if (cart != null)
            {
                cartItemViewModel.CartItems = cart;
                cartItemViewModel.GrandTotal = cart.Sum(x => x.Quantity * x.Price);
                cartItemViewModel.TotalQT = cart.Sum(x => x.Quantity);
            }
            else
            {
                cartItemViewModel.CartItems = new List<CartModel>();
                cartItemViewModel.GrandTotal = 0;
                cartItemViewModel.TotalQT = 0;
			}
			return View(cartItemViewModel);
        }
    }
}
