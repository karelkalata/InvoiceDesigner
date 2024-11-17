using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;

namespace InvoiceDesigner.Domain.Shared.DTOs
{
	public class InfoForNewInvoiceDto
	{
		public IReadOnlyCollection<CompanyAutocompleteDto> Companies { get; set; } = null!;

		public IReadOnlyCollection<CurrencyAutocompleteDto> Currencies { get; set; } = null!;

		public IReadOnlyCollection<BankAutocompleteDto> FileredBanks { get; set; } = new List<BankAutocompleteDto>();
	}
}
