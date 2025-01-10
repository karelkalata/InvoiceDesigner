using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Domain.Shared.DTOs.Bank
{
	public class BankAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int CurrencyId { get; set; }

		public int CompanyId { get; set; }

	}
}
