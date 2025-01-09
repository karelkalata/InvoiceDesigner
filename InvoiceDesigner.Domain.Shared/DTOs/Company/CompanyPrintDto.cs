using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Company
{
	public class CompanyPrintDto : IPrintable
	{
		public string Name { get; set; } = string.Empty;

		public string TaxId { get; set; } = string.Empty;

		public string VatId { get; set; } = string.Empty;

		public string WWW { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string Phone { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		public string Info { get; set; } = string.Empty;

		public int PaymentTerms { get; set; }

		public string GetSelectorName()
		{
			return "Company";
		}

		public override string ToString()
		{
			return Name;
		}

	}
}
