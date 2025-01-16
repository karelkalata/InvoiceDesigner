using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsAccounting
{
	public class ChartOfAccounts : ABaseEntity
	{
		public int Code { get; set; }
		public ETypeChartOfAccount TypeChartOfAccount { get; set; }
		public EAssetType Asset1 { get; set; }
		public EAssetType Asset2 { get; set; }
		public EAssetType Asset3 { get; set; }
	}
}
