using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsAccounting
{
	public class ChartOfAccounts
	{
		public int Id { get; init; }

		public int Code { get; set; }

		public string Name { get; set; } = string.Empty;

		public EAssetType Asset1 { get; set; }

		public EAssetType Asset2 { get; set; }

		public EAssetType Asset3 { get; set; }
	}
}
