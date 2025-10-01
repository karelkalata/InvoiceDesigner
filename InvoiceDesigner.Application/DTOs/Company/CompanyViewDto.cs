using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.DTOs.Company
{
	public class CompanyViewDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public bool IsDeleted { get; set; }

		public string TaxId { get; set; } = string.Empty;

		public string CurrencyName { get; set; } = string.Empty;

	}
}
