using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Bank
{
	public class BankEditDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(200, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Name { get; set; } = string.Empty;

		[StringLength(11, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string BIC { get; set; } = string.Empty;

		[Required]
		[StringLength(50, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Account { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "Currency  is required.")]
		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		public int CompanyId { get; set; }

	}
}
