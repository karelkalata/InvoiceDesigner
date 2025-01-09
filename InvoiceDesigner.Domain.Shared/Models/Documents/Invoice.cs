using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class Invoice : AAccountingDocument
	{
		public DateTime DueDate { get; set; }

		public string PONumber { get; set; } = null!;

		public decimal Vat { get; set; } = decimal.Zero;

		public bool EnabledVat { get; set; } = true;

		public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

		public override decimal GetAmountTax()
		{
			return InvoiceItems.Sum(item => item.Price * item.Quantity) / 100 * Vat;
		}

		public override decimal GetAmountWithoutTax()
		{
			return InvoiceItems.Sum(item => item.Price * item.Quantity);
		}

	}
}
