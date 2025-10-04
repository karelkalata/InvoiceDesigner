using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.DTOs.Company
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
