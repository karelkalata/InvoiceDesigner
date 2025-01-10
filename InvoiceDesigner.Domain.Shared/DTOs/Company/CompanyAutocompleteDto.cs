using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Domain.Shared.DTOs.Company
{
	public class CompanyAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int PaymentTerms { get; set; }

		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		public decimal DefaultVat { get; set; } = 21;

		public ICollection<BankAutocompleteDto> Banks { get; set; } = new List<BankAutocompleteDto>();

	}
}
