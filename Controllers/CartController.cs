using System.Security.Claims;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShopQuanAo.Data;
using ShopQuanAo.Infrastructure;
using ShopQuanAo.Models;

namespace ShopQuanAo.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View("Cart",HttpContext.Session.GetJson<Cart>("cart"));
        //}
        public  IActionResult Add(int Id)
        {
            Product? product =  _context.products.FirstOrDefault(p => p.Id == Id);
            List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
            CartModel cartModel=cart.Where(c=>c.ProductId==Id).FirstOrDefault();
            if (cartModel == null)
            {
                cart.Add(new CartModel(product));
            }
            else
            {
                cart.Where(c => c.ProductId == Id).FirstOrDefault().Quantity+=1;
                //cartModel.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Add item successfully ";



            //return Redirect(Request.Headers["Referer"].ToString());
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Index()
        {
            List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart")??new List<CartModel> ();
            CartItemViewModel cartItemViewModel = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity*(1-x.Discout) * x.Price),
            };

            return View("Cart",cartItemViewModel);
        }
        //public IActionResult AddToCart(int ProductId)
        //{
        //    Product? product = _context.products.FirstOrDefault(p => p.Id == ProductId);
        //    if (product != null)
        //    {
        //        Cart=HttpContext.Session.GetJson<Cart>("cart")??new Cart();
        //        Cart.AddItem(product, 1);
        //        HttpContext.Session.SetJson("cart", Cart);
        //    }
        //    return View("Cart",Cart);
        //}
        public IActionResult Increase(int Id)
        {
            Product? product = _context.products.FirstOrDefault(p => p.Id == Id);
            List<CartModel> cartlist = HttpContext.Session.GetJson<List<CartModel>>("Cart");

            if (product != null)
            {
                cartlist.Where(p => p.ProductId == Id).FirstOrDefault().Quantity+=1;

               
                HttpContext.Session.SetJson("Cart", cartlist);
            }
            TempData["success"] = "Increase item successfully";

            return RedirectToAction("Index");
        }
        public IActionResult Decrease(int Id)
        {
            Product? product = _context.products.FirstOrDefault(p => p.Id == Id);
            List<CartModel> cartlist = HttpContext.Session.GetJson<List<CartModel>>("Cart");

            if (product != null&& cartlist.Where(p => p.ProductId == Id).FirstOrDefault().Quantity > 1)
            {
                cartlist.Where(p => p.ProductId == Id).FirstOrDefault().Quantity -= 1;

            }
            else
            {
                cartlist.RemoveAll(p => p.ProductId == Id);
            }
            if(cartlist.Count==0)
            {
                HttpContext.Session.Remove("Cart");

            }
            else
            {
                HttpContext.Session.SetJson("Cart", cartlist);

            }
            TempData["success"] = "Decrease item successfully";

            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int Id)
        {
            Product? product = _context.products.FirstOrDefault(p => p.Id == Id);
            List<CartModel> cartlist = HttpContext.Session.GetJson<List<CartModel>>("Cart");
            if (product != null)
            {
                cartlist.RemoveAll(p => p.ProductId == Id);
            }
            if(cartlist.Count==0)
            {
                HttpContext.Session.Remove("Cart");

            }
            else
            {
                HttpContext.Session.SetJson("Cart", cartlist);

            }
            TempData["success"] = "Remove item successfully";

            return RedirectToAction("Index");

        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Clear item successfully";

            return RedirectToAction("Index");

        } 
        public async Task <IActionResult> CheckOut()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);   
            if(userEmail==null)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                var ordercode=Guid.NewGuid().ToString();
                var orderitem = new OrderModel();
                orderitem.Order_Code= ordercode;
                orderitem.UserName= userEmail;
                orderitem.CreateDate= DateTime.Now;
                orderitem.Status = 1;
                   _context.Add(orderitem);
                _context.SaveChanges();
                List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
                foreach(var cartitem in cart)
                {
                    var orderdetail = new OrderDetail();
                    orderdetail.UserName= userEmail;
                    orderdetail.OrderCode= ordercode;
                    orderdetail.ProductId=cartitem.ProductId;
                    //orderdetail.Product.Name = cartitem.ProductName;
                    orderdetail.Price=cartitem.Price*(1-cartitem.Discout);
                    orderdetail.Quantity=cartitem.Quantity;
                    //orderdetail.OrderId = orderitem.Id;
                    _context.Add(orderdetail);
                    _context.SaveChanges();
                }    
                TempData["success"] = "Đơn hàng đã được tạo thành công";

                HttpContext.Session.Remove("Cart");


                return RedirectToAction("Index","Cart");

            }
        }
    }
}
