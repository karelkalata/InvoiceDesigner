using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.User
{
	public class UserEditDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Name { get; set; } = string.Empty;

		[StringLength(50, MinimumLength = 5, ErrorMessage = "The {0} field must be exactly {1} characters long.")]
		public string? Password { get; set; } = null;

	}
}
