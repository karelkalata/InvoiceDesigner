using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Company
{
	public class CompanyEditDto
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Company name is required.")]
		[StringLength(300, ErrorMessage = "Vat ID cannot be longer than 50 characters.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Vat ID is required.")]
		[StringLength(50, ErrorMessage = "Vat ID cannot be longer than 50 characters.")]
		public string TaxId { get; set; } = null!;

		public string? VatId { get; set; }

		public string? WWW { get; set; }

		public string? Email { get; set; }

		public string? Phone { get; set; }

		public string? Address { get; set; }

		public string? Info { get; set; }

		[Required(ErrorMessage = "Company is required.")]
		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		[Range(1, 365, ErrorMessage = "Payment terms must be between 1 and 365 days.")]
		public int PaymentTerms { get; set; } = 14;

		[Range(0, 100, ErrorMessage = "Default VAT must be between 0% and 100%.")]
		public decimal DefaultVat { get; set; } = 21;

		public List<BankEditDto> Banks { get; set; } = new List<BankEditDto>();
	}

}
