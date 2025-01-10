using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs
{
	public class ChartOfAccountsAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
