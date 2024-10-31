using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceItem;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Invoice
{
	public class InvoiceEditDto
	{
		public int Id { get; set; }

		public int InvoiceNumber { get; set; }

		public string PONumber { get; set; } = string.Empty;

		public decimal Vat { get; set; }

		public bool EnabledVat { get; set; } = true;

		public DateTime? DateTime { get; set; }

		public DateTime? DueDate { get; set; }

		[Required]
		public CustomerAutocompleteDto Customer { get; set; } = null!;

		[Required]
		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		[Required]
		public CompanyAutocompleteDto Company { get; set; } = null!;

		[Required]
		public BankAutocompleteDto Bank { get; set; } = null!;

		public List<InvoiceItemDto> InvoiceItems { get; set; } = new();

		public decimal TotalAmount { get; set; }

		public int FormDesignerId { get; set; } = 0;
	}
}

