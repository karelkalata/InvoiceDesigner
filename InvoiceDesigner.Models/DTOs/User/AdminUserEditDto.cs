using InvoiceDesigner.Domain.Shared.DTOs.Company;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.User
{
	public class AdminUserEditDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Login { get; set; } = string.Empty;

		[Required]
		[StringLength(100, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Name { get; set; } = string.Empty;

		public bool IsAdmin { get; set; }

		[StringLength(50, MinimumLength = 5, ErrorMessage = "The {0} field must be exactly {1} characters long.")]
		public string? Password { get; set; } = null;

		public ICollection<CompanyAutocompleteDto> Companies { get; set; } = new List<CompanyAutocompleteDto>();

	}
}
