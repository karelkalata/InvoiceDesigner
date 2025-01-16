using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs
{
	public class ChartOfAccountsDto
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public string Name { get; set; } = string.Empty;
		public ETypeChartOfAccount TypeChartOfAccount { get; set; }
		public EAssetType Asset1 { get; set; }
		public EAssetType Asset2 { get; set; }
		public EAssetType Asset3 { get; set; }
	}
}
