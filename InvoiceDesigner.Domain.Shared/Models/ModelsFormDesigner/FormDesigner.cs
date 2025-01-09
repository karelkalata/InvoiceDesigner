using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner
{
	public class FormDesigner
	{
		public int Id { get; init; }

		public EAccountingDocument AccountingDocument { get; set; }

		public string Name { get; set; } = string.Empty;

		public List<FormDesignerScheme> Schemes { get; set; } = new List<FormDesignerScheme>();

		public List<DropItem> DropItems { get; set; } = new List<DropItem>();

	}
}
