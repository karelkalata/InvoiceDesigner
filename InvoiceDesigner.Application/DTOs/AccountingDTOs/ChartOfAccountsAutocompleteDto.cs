using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.DTOs.AccountingDTOs
{
	public class ChartOfAccountsAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public string Name { get; set; } = string.Empty;
		public EAssetType Asset1 { get; set; }
		public EAssetType Asset2 { get; set; }
		public EAssetType Asset3 { get; set; }
	}
}
