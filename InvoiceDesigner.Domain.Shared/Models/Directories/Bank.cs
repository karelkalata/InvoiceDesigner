using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Bank : ABaseEntity
	{
		public string BIC { get; set; } = null!;
		public string Account { get; set; } = null!;
		public string Address { get; set; } = string.Empty;
		public int CurrencyId { get; set; }
		public Currency Currency { get; set; } = null!;
		public int CompanyId { get; set; }
		public Company Company { get; set; } = null!;
	}
}
