using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopQuanAo.Data;
using ShopQuanAo.Models;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class AccountController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;

        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            
        }
        public async Task<ActionResult> IndexAsync()
        {
            List<UserModel> users= new List<UserModel>();
              foreach(var user in _userManager.Users)
            {
         var roles = await _userManager.GetRolesAsync(user);

                users.Add(new UserModel
                {
                    Name = user.UserName,
                    Email = user.Email,
                    Roles = roles.FirstOrDefault()
                });

            }    

            return _userManager != null ?
                        View(users) :
                        Problem("Entity set 'ApplicationDbContext.categories'  is null.");
        }
        public IActionResult Create()
        {
           return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {

            await AddRole(user.Roles);
            if (ModelState.IsValid)
            {
                AppUserModel newuser = new AppUserModel { UserName = user.Name, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newuser, user.Password);
                IdentityResult identity = await _userManager.AddToRoleAsync(newuser, user.Roles);

                if (result.Succeeded && identity.Succeeded)
                {
                    TempData["success"] = "tạo user thành công";
                    return Redirect("/admin/account");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
        public async Task<IActionResult> Delete(string? name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
            {
                return NotFound();

            }
            var role = await _userManager.GetRolesAsync(user);
            UserModel model = new UserModel();
            model.Name = user.UserName; model.Email = user.Email;
            model.Roles = role.FirstOrDefault();

            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string name)
        {
            var users = await _userManager.FindByNameAsync(name);
            
            if (users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            

            await _userManager.DeleteAsync(users);
            TempData["error"] = "đã xóa tài khoản ";
            return RedirectToAction(nameof(Index));
        }



        public async Task AddRole(string roles)
        {
            if (!await _roleManager.RoleExistsAsync(roles))
            {
                var role = new IdentityRole(roles);
                var result = await _roleManager.CreateAsync(role);



            }
        }

    }
}
