using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Application.DTOs.InvoiceDTOs
{
	public class InvoicePrintDto : IPrintable
	{
		public int Number { get; set; } = 0;

		public EStatus Status { get; set; }

		public string PONumber { get; set; } = string.Empty;

		public DateTime DateTime { get; set; }

		public DateTime DueDate { get; set; }

		public decimal Vat { get; set; } = decimal.Zero;

		public bool EnabledVat { get; set; }

		public decimal Amount { get; set; } = decimal.Zero;

		public CompanyPrintDto Company { get; set; } = null!;

		public CustomerPrintDto Customer { get; set; } = null!;

		public CurrencyPrintDto Currency { get; set; } = null!;

		public BankPrintDto Bank { get; set; } = null!;

		public List<InvoiceItemPrintDto> InvoiceItems { get; set; } = new();

		public string GetSelectorName()
		{
			return "Invoice";
		}

	}
}
