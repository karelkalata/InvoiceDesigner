using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.DTOs.User
{
	public class UserLoginDto
	{
		[Required]
		public string Login { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

	}
}
