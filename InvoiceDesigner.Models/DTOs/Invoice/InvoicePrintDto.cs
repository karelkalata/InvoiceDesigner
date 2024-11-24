using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Invoice
{
	public class InvoicePrintDto : IPrintable
	{
		public int InvoiceNumber { get; set; } = 0;

		public EInvoiceStatus Status { get; set; }

		public string PONumber { get; set; } = string.Empty;

		public DateTime DateTime { get; set; }

		public DateTime DueDate { get; set; }

		public decimal Vat { get; set; } = decimal.Zero;

		public bool EnabledVat { get; set; }

		public decimal TotalAmount { get; set; } = decimal.Zero;

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
