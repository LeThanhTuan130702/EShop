using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
	public class LoginViewModel
	{


		public int Id { get; set; }
		[Required(ErrorMessage = "Nhập username")]
		public string Name { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Nhập mật khẩu")]
		public string Password { get; set; }
		public string? ReturnUrl { get; set; }
	}
}
