using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Currency
{
	public class CurrencyPrintDto : IPrintable
	{
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string CurrencySymbol { get; set; } = string.Empty;

		public string GetSelectorName()
		{
			return "Currency";
		}

		public override string ToString()
		{
			return Name;
		}

	}
}
