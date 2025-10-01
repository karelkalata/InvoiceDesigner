using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.DTOs.Currency;

namespace InvoiceDesigner.Application.DTOs
{
	public class InfoForNewInvoiceDto
	{
		public IReadOnlyCollection<CompanyAutocompleteDto> Companies { get; set; } = null!;
		public IReadOnlyCollection<CurrencyAutocompleteDto> Currencies { get; set; } = null!;
		public IReadOnlyCollection<BankAutocompleteDto> FileredBanks { get; set; } = new List<BankAutocompleteDto>();
	}
}
