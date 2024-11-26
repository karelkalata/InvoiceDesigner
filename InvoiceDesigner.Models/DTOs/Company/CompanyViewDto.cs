using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Company
{
	public class CompanyViewDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string TaxId { get; set; } = string.Empty;

		public string CurrencyName { get; set; } = string.Empty;

	}
}
