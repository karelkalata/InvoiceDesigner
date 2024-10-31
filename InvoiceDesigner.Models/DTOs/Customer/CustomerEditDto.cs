using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Customer
{
	public class CustomerEditDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(200, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string Name { get; set; } = null!;

		[StringLength(50, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string TaxId { get; set; } = string.Empty;

		[StringLength(50, ErrorMessage = "The {0} field must be a maximum of {1} characters long.")]
		public string VatId { get; set; } = string.Empty;
	}
}
