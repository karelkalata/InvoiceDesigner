using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.DTOs.Invoice
{
	public class InvoiceEditDto
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public EStatus Status { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		public string PONumber { get; set; } = string.Empty;
		public decimal Vat { get; set; }
		public bool EnabledVat { get; set; } = true;
		public DateTime? DateTime { get; set; }
		public DateTime? DueDate { get; set; }

		[Required]
		public CustomerAutocompleteDto Customer { get; set; } = null!;

		[Required]
		public BankAutocompleteDto Bank { get; set; } = null!;

		[Required]
		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		[Required]
		public CompanyAutocompleteDto Company { get; set; } = null!;

		public List<InvoiceItemDto> InvoiceItems { get; set; } = new List<InvoiceItemDto>();

		public decimal Amount { get; set; }
	}
}

