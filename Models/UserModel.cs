using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Models
{
	public class UserModel
	{
		
		public int Id { get; set; }
		[Required(ErrorMessage ="Nhập username")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Nhập Email"),EmailAddress]

		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Nhập mật khẩu")]
		public string Password { get; set; }
		public string? Roles { get; set; }

	}
}
