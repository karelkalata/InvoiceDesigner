using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class BankReceipt : AAccountingDocument
	{
		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; } = null!;

		public override decimal GetAmountTax()
		{
			return Invoice.GetAmountTax();
		}

		public override decimal GetAmountWithoutTax()
		{
			return Invoice.GetAmountWithoutTax();
		}

	}
}
