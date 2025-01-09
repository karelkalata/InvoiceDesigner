using InvoiceDesigner.Domain.Shared.Models.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class InvoiceItem : AAccountingItem
	{
		public Product Item { get; set; } = null!;

		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; } = null!;

		public override decimal GetAmountWithoutTax()
		{
			return Price * Quantity;
		}
	}
}
