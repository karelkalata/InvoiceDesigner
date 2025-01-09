using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Bank
{
	public class BankPrintDto : IPrintable
	{
		public string Name { get; set; } = string.Empty;

		public string BIC { get; set; } = string.Empty;

		public string Account { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		public string GetSelectorName()
		{
			return "Bank";
		}
		public override string ToString()
		{
			return Name;
		}

	}
}
