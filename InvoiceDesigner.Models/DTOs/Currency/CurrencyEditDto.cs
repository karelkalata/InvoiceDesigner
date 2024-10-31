using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Currency
{
	public class CurrencyEditDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(3, MinimumLength = 3, ErrorMessage = "The {0} field must be exactly {1} characters long.")]
		[RegularExpression("^[A-Za-z]+$", ErrorMessage = "The {0} field can only contain letters.")]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(100, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Description { get; set; } = string.Empty;
	}
}
