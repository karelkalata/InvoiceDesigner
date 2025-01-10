using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Customer : ABaseEntity
	{
		public string? TaxId { get; set; }
		public string? VatId { get; set; }
	}
}
