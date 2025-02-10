using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner
{
	public class FormDesigner
	{
		public int Id { get; init; }
		public string Name { get; set; } = string.Empty;
		public EAccountingDocument AccountingDocument { get; set; }
		public EPageSizes PageSizes { get; set; }
		public bool DynamicHeight { get; set; } = false;
		public int PageMargin { get; set; }
		public EPageOrientation PageOrientation { get; set; }
		public List<FormDesignerScheme> Schemes { get; set; } = new List<FormDesignerScheme>();
		public List<DropItem> DropItems { get; set; } = new List<DropItem>();
	}
}
