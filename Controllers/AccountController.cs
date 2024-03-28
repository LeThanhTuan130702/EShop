using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopQuanAo.Models;

namespace ShopQuanAo.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		private RoleManager<IdentityRole> _roleManager;
		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			

		}

		public IActionResult Index(string returnUrl)
		{
			return View(new LoginViewModel
			{
				ReturnUrl = returnUrl
			});
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginView)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginView.Name, loginView.Password, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginView.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Invalid UserName and Password");
			}
			return View("Index", loginView);
		}
		public IActionResult Create()
		{
			return View();

		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			user.Roles = "member";
            await AddRole("member");
			if (ModelState.IsValid)
			{
				AppUserModel newuser = new AppUserModel { UserName = user.Name, Email = user.Email };
				IdentityResult result = await _userManager.CreateAsync(newuser, user.Password);
				IdentityResult identity = await _userManager.AddToRoleAsync(newuser, "member");

				if (result.Succeeded && identity.Succeeded)
				{
					TempData["success"] = "tạo user thành công";
					return Redirect("/account");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View();
		}
		public async Task<IActionResult> Logout(string returnurl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnurl);
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
