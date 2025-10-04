using InvoiceDesigner.Application.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Mapper
{
	public static class InvoiceMapper
	{
		public static IReadOnlyCollection<InvoicesViewDto> ToViewDto(IReadOnlyCollection<Invoice> invoices) => invoices.Select(MapToInvoicesViewDto).ToList();

		public static InvoicesViewDto MapToInvoicesViewDto(Invoice invoice)
		{
			return new InvoicesViewDto
			{
				Id = invoice.Id,
				Number = invoice.Number,
				Status = invoice.Status,
				IsDeleted = invoice.IsDeleted,
				IsArchived = invoice.IsArchived,
				DateTime = invoice.DateTime,
				DueTime = invoice.DueDate,
				CompanyName = invoice.Company?.Name ?? string.Empty,
				CustomerName = invoice.Customer?.Name ?? string.Empty,
				CurrencyName = invoice.Currency?.Name ?? string.Empty,
				Amount = invoice.Amount
			};
		}

		public static InvoiceEditDto ToEditDto(Invoice invoice)
		{
			return new InvoiceEditDto
			{
				Id = invoice.Id,
				Number = invoice.Number,
				Status = invoice.Status,
				IsDeleted = invoice.IsDeleted,
				IsArchived = invoice.IsArchived,
				PONumber = invoice.PONumber,
				Vat = invoice.Vat,
				EnabledVat = invoice.EnabledVat,
				DateTime = invoice.DateTime,
				DueDate = invoice.DueDate,
				Amount = invoice.Amount,
				Customer = CustomerMapper.ToAutocompleteDto(invoice.Customer),
				Bank = BankMapper.ToAutocompleteDto(invoice.Bank),
				Currency = CurrencyMapper.ToAutocompleteDto(invoice.Currency),
				Company = CompanyMapper.ToAutocompleteDto(invoice.Company),
				InvoiceItems = invoice.InvoiceItems.Select(InvoiceItemMapper.ToEditDto).ToList()
			};
		}

		public static InvoicePrintDto ToPrintDto(Invoice invoice)
		{
			return new InvoicePrintDto
			{
				Number = invoice.Number,
				Status = invoice.Status,
				PONumber = invoice.PONumber,
				DateTime = invoice.DateTime,
				DueDate = invoice.DueDate,
				Vat = invoice.Vat,
				EnabledVat = invoice.EnabledVat,
				Amount = invoice.Amount,
				Company = CompanyMapper.ToPrintDto(invoice.Company),
				Customer = CustomerMapper.ToPrintDto(invoice.Customer),
				Currency = CurrencyMapper.ToPrintDto(invoice.Currency),
				Bank = BankMapper.ToPrintDto(invoice.Bank),
				InvoiceItems = invoice.InvoiceItems.Select(InvoiceItemMapper.ToPrintDto).ToList()
			};
		}
	}
}
