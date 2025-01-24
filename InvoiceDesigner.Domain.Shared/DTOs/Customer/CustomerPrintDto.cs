using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Customer
{
	public class CustomerPrintDto : IPrintable
	{
		public string Name { get; set; } = string.Empty;

		public string TaxId { get; set; } = string.Empty;

		public string VatId { get; set; } = string.Empty;

		public string GetSelectorName()
		{
			return "CustomerId";
		}
		public override string ToString()
		{
			return Name;
		}

	}
}
